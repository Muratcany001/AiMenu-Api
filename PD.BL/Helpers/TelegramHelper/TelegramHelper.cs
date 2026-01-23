using Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace PD.BL.Helpers.TelegramHelper
{
    public class TelegramHelper : ITelegramHelper
    {
        private readonly TelegramBotClient _bot;
        private readonly string _chatId;

        public TelegramHelper()
        {
            
            _bot = new TelegramBotClient("8250563005:AAEpaPyJBaqxI8HUmtHzs7DGPPE0nnhZErM");
            Console.WriteLine(ApiKey.Telegram);
        }

        public async Task SendOrderCompletedAsync(OrderDto orderDto)
        {
            var message = BuildOrderCompletedMessage(orderDto);
            await _bot.SendMessage(5219802587, message);

            await _bot.SendPoll(
            chatId: 5219802587,
            question: "Sipariş Durumu",
            options: ["Sipariş Hazır","Sipariş Hazır"],
            allowsMultipleAnswers: false
        );
        }

        private string BuildOrderCompletedMessage(OrderDto order)
        {
            var sb = new StringBuilder();

            sb.AppendLine(order.Created.ToString("dd.MM.yyyy HH:mm"));
            sb.AppendLine($"Sipariş Kodu: {order.OrderNumber}");
            sb.AppendLine("Sipariş İçeriği:");

            foreach (var item in order.OrderItems)
            {
                sb.AppendLine($"- MenüId: {item.MenuItem.Name} | Adet: {item.Quantity}");
            }

            sb.AppendLine($"Toplam Ürün Adedi: {order.OrderItems.Count}");
           
            return sb.ToString();
        }
    }
}

