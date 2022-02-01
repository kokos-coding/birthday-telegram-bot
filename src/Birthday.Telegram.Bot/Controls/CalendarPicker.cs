using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Birthday.Telegram.Bot.Controls
{
    /// <summary>
    /// Calendar picker control
    /// </summary>
    public static class CalendarPicker
    {
        public static InlineKeyboardMarkup GetYearsKeyboard(DateTime centralDate)
        {

            var result = new InlineKeyboardMarkup(new []
            {
                // First line
                new []
                {
                    InlineKeyboardButton.WithCallbackData(centralDate.AddYears(-4).Year.ToString(), centralDate.AddYears(-4).ToLongDateString()),
                    InlineKeyboardButton.WithCallbackData(centralDate.AddYears(-3).Year.ToString(), centralDate.AddYears(-3).ToLongDateString()),
                    InlineKeyboardButton.WithCallbackData(centralDate.AddYears(-2).Year.ToString(), centralDate.AddYears(-2).ToLongDateString())
                },
                // Second line
                new []
                {
                    InlineKeyboardButton.WithCallbackData(centralDate.AddYears(-1).Year.ToString(), centralDate.AddYears(-1).ToLongDateString()),
                    InlineKeyboardButton.WithCallbackData(centralDate.Year.ToString(), centralDate.ToLongDateString()),
                    InlineKeyboardButton.WithCallbackData(centralDate.AddYears(1).Year.ToString(), centralDate.AddYears(1).ToLongDateString())
                },
                // Third line
                new []
                {
                    InlineKeyboardButton.WithCallbackData(centralDate.AddYears(2).Year.ToString(), centralDate.AddYears(2).ToLongDateString()),
                    InlineKeyboardButton.WithCallbackData(centralDate.AddYears(3).Year.ToString(), centralDate.AddYears(3).ToLongDateString()),
                    InlineKeyboardButton.WithCallbackData(centralDate.AddYears(4).Year.ToString(), centralDate.AddYears(4).ToLongDateString())
                }
            });

            return result;
        }
    }
}