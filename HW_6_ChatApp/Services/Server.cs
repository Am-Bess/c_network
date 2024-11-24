﻿using System.Net;
using HW_6_ChatApp.Abstraction;
using HW_6_ChatApp.Models;

namespace HW_6_ChatApp.Services
{
    class Server
    {
        private Dictionary<String, IPEndPoint> clients = new Dictionary<String, IPEndPoint>();
        IMessageSource messageSource;

        bool work = true;

        public Server(IMessageSource source)
        {
            messageSource = source;
        }

        public void Work()
        {
            Console.WriteLine("UDP Сервер ожидает сообщений...");

            while (work)
            {
                try
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var message = messageSource.Receive(ref remoteEndPoint);
                    ProcessMessage(message, remoteEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                }
            }
        }

        public void Stop()
        {
            work = false;
        }

        private void ProcessMessage(MessageUdp messageUdp, IPEndPoint fromep)
        {
            Console.WriteLine($"Получено сообщение от {messageUdp.FromName} для {messageUdp.ToName} с командой {messageUdp.Command}:");
            Console.WriteLine(messageUdp?.Text);

            if (messageUdp?.Command == Command.Register)
            {
                Register(messageUdp, new IPEndPoint(fromep.Address, fromep.Port));
            }
            if (messageUdp?.Command == Command.Confirmation)
            {
                Console.WriteLine("Confirmation receiver");
                ConfirmMessageReceived(messageUdp.Id);
            }
            if (messageUdp?.Command == Command.Message)
            {
                RelyMessage(messageUdp);
            }

        }

        private void RelyMessage(MessageUdp messageUdp)
        {
            if (clients.TryGetValue(messageUdp.ToName!, out IPEndPoint? ep))
            {
                int id;

                using (var context = new Context())
                {
                    var fromUser = context.Users?.FirstOrDefault((u) => u.Name == messageUdp.FromName);
                    var toUser = context.Users?.FirstOrDefault((u) => u.Name == messageUdp.ToName);

                    var messageDd = new Message()
                    {
                        Text = messageUdp.Text,
                        DateMessage = DateTime.Now,
                        IsReceived = false,
                        ToUser = toUser,
                        FromUser = fromUser
                    };
                    context.Messages?.Add(messageDd);
                    context.SaveChanges();
                    id = messageDd.Id;
                }

                var forwardMessage = new MessageUdp()
                {
                    Id = id,
                    Command = Command.Message,
                    ToName = messageUdp.ToName,
                    FromName = messageUdp.FromName,
                    Text = messageUdp.Text
                };

                messageSource.Send(forwardMessage, ep);

                Console.WriteLine($"Message Relied, from = {messageUdp.FromName} to = {messageUdp.ToName}");
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }

        void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id=" + id);

            using (var context = new Context())
            {
                var msg = context.Messages?.FirstOrDefault(x => x.Id == id);

                if (msg != null)
                {
                    msg.IsReceived = true;
                    context.SaveChanges();
                }
            }
        }


        private void Register(MessageUdp messageUdp, IPEndPoint fromep)
        {
            Console.WriteLine($"Message register, Name = {messageUdp.FromName}");
            clients?.Add(messageUdp.FromName!, fromep);

            using (var context = new Context())
            {
                if (context.Users?.FirstOrDefault(x => x.Name == messageUdp.FromName) != null) return;
                context.Add(new User { Name = messageUdp.FromName });
                context.SaveChanges();
            }
        }
    }
}