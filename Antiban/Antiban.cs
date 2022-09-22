using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiban
{
    public class Antiban
    {
        private List<EventMessage> Messages { get; } = new();

        /// <summary>
        /// Добавление сообщений в систему, для обработки порядка сообщений
        /// </summary>
        /// <param name="eventMessage"></param>
        public void PushEventMessage(EventMessage eventMessage)
        {
            //TODO
            var lastSendedNumber = Messages.LastOrDefault(m => m.DateTime <= eventMessage.DateTime);
            var lastMessageToNumber = Messages.LastOrDefault(m => m.Phone == eventMessage.Phone && m.DateTime <= eventMessage.DateTime);

            var tempDateTime = eventMessage.DateTime;

            if (lastSendedNumber != null && lastSendedNumber.DateTime.AddSeconds(10) >= eventMessage.DateTime) tempDateTime = lastSendedNumber.DateTime.AddSeconds(10);
            if (lastMessageToNumber != null && lastMessageToNumber.DateTime.AddMinutes(1) >= eventMessage.DateTime) tempDateTime = lastMessageToNumber.DateTime.AddMinutes(1);

            if (eventMessage.Priority == 1)
            {
                var lastPriorityMessageToNumber =
                    Messages.LastOrDefault(m => m.Phone == eventMessage.Phone && m.Priority == 1);

                if (lastPriorityMessageToNumber != null && lastPriorityMessageToNumber.DateTime.AddHours(24) >= eventMessage.DateTime)
                {
                    tempDateTime = lastPriorityMessageToNumber.DateTime.AddHours(24);
                }
            }

            while (Messages.Any(m => m.DateTime == tempDateTime))
            {
                tempDateTime = tempDateTime.AddSeconds(10);
            }

            eventMessage.DateTime = tempDateTime;
            Messages.Add(eventMessage);


        }

        /// <summary>
        /// Вовзращает порядок отправок сообщений
        /// </summary>
        /// <returns></returns>
        public List<AntibanResult> GetResult()
        {
            //TODO
            //Example
            var results = new List<AntibanResult>();

            foreach (var message in Messages)
            {
                var result = new AntibanResult()
                {
                    EventMessageId = message.Id,
                    SentDateTime = message.DateTime
                };

                results.Add(result);
            }

            return results.OrderBy(x => x.SentDateTime).ToList();
        }
    }
}
