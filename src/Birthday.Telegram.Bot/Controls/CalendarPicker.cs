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
    public static string CalendarPickerCommand => "/calendar-picker";

    public static bool IsCalendarPickerCommand(string command) => 
        command.StartsWith(CalendarPickerCommand);

    public static InlineKeyboardMarkup? CalendarPickerProcessor(string calendarPickerCommand)
    {
        var subCalendarPickerCommand = calendarPickerCommand
                .TrimStart(CalendarPickerCommand.ToCharArray())
                .Split(' ')
                .ToArray();
        if(subCalendarPickerCommand.Length != 2)
            return null;
        
        var subCommand = subCalendarPickerCommand.First();
        if(DateTime.TryParse(subCalendarPickerCommand.Last(), out var currentDate))
        {
            var action = subCommand switch
            {
                CalendarPickerSubCommands.YearBack => GetYearsKeyboard(currentDate.AddYears(-9)),
                CalendarPickerSubCommands.YearForward => GetYearsKeyboard(currentDate.AddYears(9)),
                CalendarPickerSubCommands.MonthBack => GetCalendarKeyboard(currentDate.AddMonths(-1)),
                CalendarPickerSubCommands.MonthForward => GetCalendarKeyboard(currentDate.AddMonths(1)),
                CalendarPickerSubCommands.SetYear => "",
                _ => throw new NotImplementedException(),
            };
            return action;
        }
        return null;
    }

    public static InlineKeyboardMarkup InitializeCalendarPickerKeyboard(DateTime currentDateTime) => GetYearsKeyboard(currentDateTime);

    private static class CalendarPickerSubCommands
    {
        public const string YearBack = "year-back";
        public const string YearForward = "year-forward";
        public const string MonthBack = "month-back";
        public const string MonthForward = "month-forward";
        public const string SetYear = "set-year";
    }

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
        if (nowDate.Year < centralDate.Year)
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
                    new InlineKeyboardButton("..."),
                    InlineKeyboardButton.WithCallbackData(">", $"{CalendarPickerCommand} {CalendarPickerSubCommands.YearForward} {centralDate.ToShortDateString()}")
            } : showRightArrow ?
            new InlineKeyboardButton[]
            {
                    InlineKeyboardButton.WithCallbackData("<", $"{CalendarPickerCommand} {CalendarPickerSubCommands.YearBack} {centralDate.ToShortDateString()}"),
                    new InlineKeyboardButton("..."),
                    new InlineKeyboardButton("..."),
            } :
            new InlineKeyboardButton[]
            {
                    new InlineKeyboardButton("..."),
                    new InlineKeyboardButton("..."),
                    InlineKeyboardButton.WithCallbackData(">", $"{CalendarPickerCommand} {CalendarPickerSubCommands.YearForward} {centralDate.ToShortDateString()}")
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
        
        var result = new InlineKeyboardMarkup(new InlineKeyboardButton[] { });

        return result;
    }

    private static class Months
    {
        private static ItemInfo January => new("Январь", "Янв");
        private static ItemInfo February => new("Февраль", "Феб");
        private static ItemInfo March => new("Март", "Мар");
        private static ItemInfo April => new("Апрель", "Апр");
        private static ItemInfo May => new("Май", "Май");
        private static ItemInfo June => new("Июнь", "Июн");
        private static ItemInfo July => new("Июль", "Июл");
        private static ItemInfo August => new("Август", "Авг");
        private static ItemInfo September => new("Сентябрь", "Сен");
        private static ItemInfo October => new("Октябрь", "Окт");
        private static ItemInfo November => new("Ноябрь", "Ноя");
        private static ItemInfo December => new("Декабрь", "Дек");
    }

    private static Dictionary<DayOfWeek, ItemInfo> DayOfWeeks => new()
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