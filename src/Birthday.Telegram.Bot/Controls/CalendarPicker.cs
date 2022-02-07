using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Birthday.Telegram.Bot.Controls;

/// <summary>
/// Calendar picker control
/// </summary>
public static class CalendarPicker
{
    /// <summary>
    /// Минимально возможная дата
    /// </summary>
    public static DateTime MinimalDate = new(1800, 01, 01);
    public static string CalendarPickerCommand => "/calendarpicker";
    private static class CalendarPickerSubCommands
    {
        public const string GetYearsKeyboard = "yearskeyboard";
        public const string YearBack = "yearback";
        public const string YearForward = "yearforward";
        public const string SetYear = "setyear";
        public const string MonthBack = "monthback";
        public const string MonthForward = "monthforward";
        public const string SetDate = "setdate";
    }

    private const string EmptyInlineKeyboardButton = " ";

    public static bool IsCalendarPickerCommand(string command, CultureInfo cultureInfo) =>
        command.StartsWith(CalendarPickerCommand, false, cultureInfo);

    public static InlineKeyboardMarkup? CalendarPickerProcessor(string calendarPickerCommand)
    {
        var subCalendarPickerCommand = calendarPickerCommand
                .TrimStart(CalendarPickerCommand.ToCharArray())
                .Trim(' ')
                .Split(' ')
                .ToArray();
        if (subCalendarPickerCommand.Length != 2)
            return null;

        var subCommand = subCalendarPickerCommand.First();
        if (DateTime.TryParse(subCalendarPickerCommand.Last(), out var currentDate))
        {
            var action = subCommand switch
            {
                CalendarPickerSubCommands.GetYearsKeyboard => GetYearsKeyboard(currentDate),
                CalendarPickerSubCommands.YearBack => GetYearsKeyboard(currentDate.AddYears(-9)),
                CalendarPickerSubCommands.YearForward => GetYearsKeyboard(currentDate.AddYears(9)),
                CalendarPickerSubCommands.MonthBack => GetCalendarKeyboard(currentDate.AddMonths(-1)),
                CalendarPickerSubCommands.MonthForward => GetCalendarKeyboard(currentDate.AddMonths(1)),
                CalendarPickerSubCommands.SetYear => GetCalendarKeyboard(currentDate),
                _ => throw new NotImplementedException(),
            };
            return action;
        }
        return null;
    }

    public static InlineKeyboardMarkup InitializeCalendarPickerKeyboard(DateTime currentDateTime) => GetCalendarKeyboard(currentDateTime);

    private static InlineKeyboardMarkup GetYearsKeyboard(DateTime centralDate)
    {
        var nowDate = DateTime.Now;
        bool showLeftArrow = true;
        bool showRightArrow = true;
        if (MinimalDate > centralDate)
        {
            centralDate = new DateTime(MinimalDate.Year, centralDate.Month, centralDate.Day);
            showLeftArrow = false;
        }
        if (nowDate.Year <= centralDate.Year)
        {
            centralDate = new DateTime(nowDate.Year, centralDate.Month, centralDate.Day);
            showRightArrow = false;
        }

        InlineKeyboardButton GetYearKeyBoardButton(DateTime centralDate, int deviation) =>
            InlineKeyboardButton.WithCallbackData(centralDate.AddYears(deviation).Year.ToString(),
                                            $"{CalendarPickerCommand} {CalendarPickerSubCommands.SetYear} {centralDate.AddYears(deviation).ToShortDateString()}");

        var bottomLine = showLeftArrow && showRightArrow ?
            new InlineKeyboardButton[]
            {
                    InlineKeyboardButton.WithCallbackData("<", $"{CalendarPickerCommand} {CalendarPickerSubCommands.YearBack} {centralDate.ToShortDateString()}"),
                    EmptyInlineKeyboardButton!,
                    InlineKeyboardButton.WithCallbackData(">", $"{CalendarPickerCommand} {CalendarPickerSubCommands.YearForward} {centralDate.ToShortDateString()}")
            } : showRightArrow ?
            new InlineKeyboardButton[]
            {
                    EmptyInlineKeyboardButton!,
                    EmptyInlineKeyboardButton!,
                    InlineKeyboardButton.WithCallbackData(">", $"{CalendarPickerCommand} {CalendarPickerSubCommands.YearForward} {centralDate.ToShortDateString()}")
            } :
            new InlineKeyboardButton[]
            {
                    InlineKeyboardButton.WithCallbackData("<", $"{CalendarPickerCommand} {CalendarPickerSubCommands.YearBack} {centralDate.ToShortDateString()}"),
                    EmptyInlineKeyboardButton!,
                    EmptyInlineKeyboardButton!,
            };
            

        var result = new InlineKeyboardMarkup(new[]
        {
                // First line
                new []
                {
                    GetYearKeyBoardButton(centralDate, -4),
                    GetYearKeyBoardButton(centralDate, -3),
                    GetYearKeyBoardButton(centralDate, -2)
                },
                // Second line
                new []
                {
                    GetYearKeyBoardButton(centralDate, -1),
                    GetYearKeyBoardButton(centralDate, 0),
                    GetYearKeyBoardButton(centralDate, 1)
                },
                // Third line
                new []
                {
                    GetYearKeyBoardButton(centralDate, 2),
                    GetYearKeyBoardButton(centralDate, 3),
                    GetYearKeyBoardButton(centralDate, 4)
                },
                bottomLine
            });

        return result;
    }

    private static InlineKeyboardMarkup GetCalendarKeyboard(DateTime currentDate)
    {
        InlineKeyboardButton[] GetHeader(DateTime currentDate)
        {
            return new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData($"{Months[CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(currentDate.Month)].Name} {currentDate.Year}",
                    $"{CalendarPickerCommand} {CalendarPickerSubCommands.GetYearsKeyboard} {currentDate.ToShortDateString()}")
            };
        }

        IEnumerable<InlineKeyboardButton> GetDaysOfWeek()
        {
            var dayNames = new InlineKeyboardButton[7];
            var keys = DaysOfWeek.Keys.ToList();
            foreach (var key in keys)
            {
                yield return DaysOfWeek[key].ShortName!;
            }
        }

        IEnumerable<InlineKeyboardButton[]> GetCalendar(DateTime currentDate)
        {
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).Day;

            for (int dayOfMonth = 1, weekNum = 0; dayOfMonth <= lastDayOfMonth; weekNum++)
            {
                yield return NewWeek(weekNum, ref dayOfMonth);
            }

            InlineKeyboardButton[] NewWeek(int weekNum, ref int dayOfMonth)
            {
                var week = new InlineKeyboardButton[7];

                for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                {
                    if ((weekNum == 0 && dayOfWeek < FirstDayOfWeek())
                       ||
                       dayOfMonth > lastDayOfMonth
                    )
                    {
                        week[dayOfWeek] = EmptyInlineKeyboardButton!;
                        continue;
                    }

                    week[dayOfWeek] = InlineKeyboardButton.WithCallbackData(
                        dayOfMonth.ToString(),
                        $"{CalendarPickerCommand} {CalendarPickerSubCommands.SetDate} {currentDate.ToShortDateString()}"
                    );

                    dayOfMonth++;
                }
                return week;

                int FirstDayOfWeek() =>
                    (7 + (int)firstDayOfMonth.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) % 7;
            }
        }

        InlineKeyboardButton[] GetButtons(DateTime currentDate) =>
            new[] {
                InlineKeyboardButton.WithCallbackData("<", $"{CalendarPickerCommand} {CalendarPickerSubCommands.MonthBack} {currentDate.ToShortDateString()}"),
                EmptyInlineKeyboardButton!,
                InlineKeyboardButton.WithCallbackData(">", $"{CalendarPickerCommand} {CalendarPickerSubCommands.MonthForward} {currentDate.ToShortDateString()}")
            };

        var resultButtons = new List<InlineKeyboardButton[]>
        {
            GetHeader(currentDate),
            GetDaysOfWeek().ToArray()
        };
        resultButtons.AddRange(GetCalendar(currentDate));
        resultButtons.Add(GetButtons(currentDate));
        return new InlineKeyboardMarkup(resultButtons);
    }

    private static Dictionary<string, ItemInfo> Months => new()
    {
        { "January", new("Январь", "Янв") },
        { "February", new("Февраль", "Феб") },
        { "March", new("Март", "Мар") },
        { "April", new("Апрель", "Апр") },
        { "May", new("Май", "Май") },
        { "June", new("Июнь", "Июн") },
        { "July", new("Июль", "Июл") },
        { "August", new("Август", "Авг") },
        { "September", new("Сентябрь", "Сен") },
        { "October", new("Октябрь", "Окт") },
        { "November", new("Ноябрь", "Ноя") },
        { "December", new("Декабрь", "Дек") }
    };

    private static Dictionary<DayOfWeek, ItemInfo> DaysOfWeek => new()
    {
        { DayOfWeek.Monday, new("Понедельник", "Пн") },
        { DayOfWeek.Tuesday, new("Вторник", "Вт") },
        { DayOfWeek.Wednesday, new("Среда", "Ср") },
        { DayOfWeek.Thursday, new("Четверг", "Чт") },
        { DayOfWeek.Friday, new("Пятница", "Пт") },
        { DayOfWeek.Saturday, new("Суббота", "Сб") },
        { DayOfWeek.Sunday, new("Воскресенье", "Вс") }
    };

    internal class ItemInfo
    {
        public ItemInfo(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }

        public string Name { get; }
        public string ShortName { get; }
    }
}