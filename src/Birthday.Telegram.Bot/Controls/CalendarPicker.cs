using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Birthday.Telegram.Bot.Controls;

/// <summary>
/// Calendar picker control
/// </summary>
public static class CalendarPicker
{
    private static CultureInfo DefaultCultureInfo = CultureInfo.CurrentCulture;

    /// <summary>
    /// Является ли данная команда, командой для работы CalendarPicker контрола
    /// </summary>
    /// <param name="command">Команда</param>
    /// <returns>Флаг результата</returns>
    public static bool IsCalendarPickerCommand(string command) =>
        command.StartsWith(Constants.MainCommand, false, DefaultCultureInfo);

    /// <summary>
    /// Является ли данная команда, командой получения даты
    /// </summary>
    /// <param name="command">Команда</param>
    /// <returns>Флаг результата</returns>
    public static bool IsCalendarPickerSetDateCommand(string command)
        => command.StartsWith($"{Constants.FullCommands.SetDate}", false, DefaultCultureInfo);

    /// <summary>
    /// Инициализация контрола с CalendarPicker
    /// </summary>
    /// <param name="currentDateTime">Текущее дата инициализации</param>
    /// <param name="cultureInfo">Текущая культура</param>
    /// <returns>Инициализированная клавиатура</returns>
    public static InlineKeyboardMarkup InitializeCalendarPickerKeyboard(DateTime currentDateTime, CultureInfo cultureInfo)
    {
        DefaultCultureInfo = cultureInfo;

        return GetYearsKeyboard(currentDateTime);
    }

    /// <summary>
    /// Процессор для обработки запросов адресованных контролу CalendarPicker
    /// </summary>
    /// <param name="calendarPickerCommand">Команда для обработки</param>
    /// <returns>Результат обработки запроса</returns>
    public static CalendarPickerProcessorResult? CalendarPickerProcessor(string calendarPickerCommand)
    {
        if (!IsCalendarPickerCommand(calendarPickerCommand)) return null;

        var subCalendarPickerCommand = Helpers.GetSubcommandAndArgument(calendarPickerCommand);
        if (subCalendarPickerCommand.Count != 2)
            return null;

        var subCommand = subCalendarPickerCommand.First();
        if (DateTime.TryParse(subCalendarPickerCommand.Last(), out var currentDate))
        {
            var action = subCommand switch
            {
                Constants.SubCommands.GetYearsKeyboard => new CalendarPickerProcessorResult()
                {
                    ResultType = ProcessorResultType.KeyboardMarkup,
                    KeyboardMarkup = GetYearsKeyboard(currentDate)
                },
                Constants.SubCommands.GetMonthsKeyboard => new CalendarPickerProcessorResult()
                {
                    ResultType = ProcessorResultType.KeyboardMarkup,
                    KeyboardMarkup = GetMonthsKeyboard(currentDate)
                },
                Constants.SubCommands.YearBack => new CalendarPickerProcessorResult()
                {
                    ResultType = ProcessorResultType.KeyboardMarkup,
                    KeyboardMarkup = GetYearsKeyboard(currentDate.AddYears(-9))
                },
                Constants.SubCommands.YearForward => new CalendarPickerProcessorResult()
                {
                    ResultType = ProcessorResultType.KeyboardMarkup,
                    KeyboardMarkup = GetYearsKeyboard(currentDate.AddYears(9))
                },
                Constants.SubCommands.MonthBack => new CalendarPickerProcessorResult()
                {
                    ResultType = ProcessorResultType.KeyboardMarkup,
                    KeyboardMarkup = GetCalendarKeyboard(currentDate.AddMonths(-1), DefaultCultureInfo)
                },
                Constants.SubCommands.MonthForward => new CalendarPickerProcessorResult()
                {
                    ResultType = ProcessorResultType.KeyboardMarkup,
                    KeyboardMarkup = GetCalendarKeyboard(currentDate.AddMonths(1), DefaultCultureInfo)
                },
                Constants.SubCommands.SetYear => new CalendarPickerProcessorResult()
                {
                    ResultType = ProcessorResultType.KeyboardMarkup,
                    KeyboardMarkup = GetMonthsKeyboard(currentDate)
                },
                Constants.SubCommands.SetMonth => new CalendarPickerProcessorResult()
                {
                    ResultType = ProcessorResultType.KeyboardMarkup,
                    KeyboardMarkup = GetCalendarKeyboard(currentDate, DefaultCultureInfo)
                },
                Constants.SubCommands.SetDate => new CalendarPickerProcessorResult()
                {
                    ResultType = ProcessorResultType.Date,
                    TargetDate = currentDate
                },
                _ => throw new NotImplementedException(),
            };
            return action;
        }
        return null;
    }

    private static InlineKeyboardMarkup GetYearsKeyboard(DateTime centralDate)
    {
        var keyboardWidth = 5;
        var keyboardHeigh = 5;
        var nowDate = DateTime.Now;
        bool showLeftArrow = true;
        bool showRightArrow = true;
        if (Constants.MinimalDate > centralDate)
        {
            centralDate = new DateTime(Constants.MinimalDate.Year, centralDate.Month, centralDate.Day);
            showLeftArrow = false;
        }
        if (nowDate.Year - 12 <= centralDate.Year)
        {
            centralDate = new DateTime(nowDate.Year - 12, centralDate.Month, centralDate.Day);
            showRightArrow = false;
        }

        static InlineKeyboardButton GetYearKeyBoardButton(DateTime centralDate, int deviation) =>
            InlineKeyboardButton.WithCallbackData(centralDate.AddYears(deviation).Year.ToString(),
                                $"{Constants.FullCommands.SetYear} {centralDate.AddYears(deviation).ToShortDateString()}");

        var bottomLine = showLeftArrow && showRightArrow ?
            new InlineKeyboardButton[]
            {
                    InlineKeyboardButton.WithCallbackData("<", $"{Constants.FullCommands.YearBack} {centralDate.ToShortDateString()}"),
                    Constants.EmptyInlineKeyboardButton!,
                    InlineKeyboardButton.WithCallbackData(">", $"{Constants.FullCommands.YearForward} {centralDate.ToShortDateString()}")
            } : showRightArrow ?
            new InlineKeyboardButton[]
            {
                    Constants.EmptyInlineKeyboardButton!,
                    Constants.EmptyInlineKeyboardButton!,
                    InlineKeyboardButton.WithCallbackData(">", $"{Constants.FullCommands.YearForward} {centralDate.ToShortDateString()}")
            } :
            new InlineKeyboardButton[]
            {
                    InlineKeyboardButton.WithCallbackData("<", $"{Constants.FullCommands.YearBack} {centralDate.ToShortDateString()}"),
                    Constants.EmptyInlineKeyboardButton!,
                    Constants.EmptyInlineKeyboardButton!,
            };

        var square = keyboardHeigh * keyboardWidth;
        var start = square % 2 == 0 ?
                    -(square / 2) - 1 :
                    -(square / 2);
        var keyboard = new List<List<InlineKeyboardButton>>();
        for (var col = 0; col < keyboardWidth; col++)
        {
            var keyboardRow = new List<InlineKeyboardButton>();
            for (var row = 0; row < keyboardHeigh; row++)
            {
                keyboardRow.Add(GetYearKeyBoardButton(centralDate, start));
                start++;
            }
            keyboard.Add(keyboardRow);
        }
        keyboard.Add(bottomLine.ToList());

        return new InlineKeyboardMarkup(keyboard);
    }

    private static InlineKeyboardMarkup GetMonthsKeyboard(DateTime centralDate)
    {
        var keyboardWidth = 4;
        var keyboardHeigh = 3;

        static InlineKeyboardButton GetMonthKeyBoardButton(string monthName, DateTime callbackDate) =>
            InlineKeyboardButton.WithCallbackData(monthName,
                                $"{Constants.FullCommands.SetMonth} {callbackDate.ToShortDateString()}");

        var monthCounter = 1;
        var date = new DateTime(centralDate.Year, monthCounter, centralDate.Day);
        var keyboard = new List<List<InlineKeyboardButton>>();
        for (var col = 0; col < keyboardWidth; col++)
        {
            var keyboardRow = new List<InlineKeyboardButton>();
            for (var row = 0; row < keyboardHeigh; row++)
            {
                var month = Helpers.GetMonthName(monthCounter);
                keyboardRow.Add(GetMonthKeyBoardButton(month, date));
                date = date.AddMonths(1);
                monthCounter++;
            }
            keyboard.Add(keyboardRow);
        }
        return new InlineKeyboardMarkup(keyboard);
    }

    private static InlineKeyboardMarkup GetCalendarKeyboard(DateTime currentDate, CultureInfo cultureInfo)
    {
        InlineKeyboardButton[] GetHeader(DateTime currentDate)
        {
            return new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData($"{Helpers.GetMonthName(currentDate)}",
                    $"{Constants.FullCommands.GetMonthsKeyboard} {currentDate.ToShortDateString()}"),
                InlineKeyboardButton.WithCallbackData($"{currentDate.Year}",
                    $"{Constants.FullCommands.GetYearsKeyboard} {currentDate.ToShortDateString()}")
            };
        }

        IEnumerable<InlineKeyboardButton> GetDaysOfWeek()
        {
            var dayNames = new InlineKeyboardButton[7];
            var keys = Constants.DaysOfWeek.Keys.ToList();
            foreach (var key in keys)
            {
                yield return Constants.DaysOfWeek[key].ShortName!;
            }
        }

        IEnumerable<InlineKeyboardButton[]> GetCalendar(DateTime currentDate, CultureInfo cultureInfo)
        {
            var lastDayOfMonth = Helpers.GetLastDayOfMonth(currentDate);

            for (int dayOfMonth = 1, weekNum = 0; dayOfMonth <= lastDayOfMonth; weekNum++)
            {
                yield return NewWeek(weekNum, ref dayOfMonth);
            }

            InlineKeyboardButton[] NewWeek(int weekNum, ref int dayOfMonth)
            {
                var week = new InlineKeyboardButton[7];

                for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                {
                    if ((weekNum == 0 && dayOfWeek < Helpers.GetFirstDayOfWeek(currentDate, cultureInfo))
                       ||
                       dayOfMonth > lastDayOfMonth
                    )
                    {
                        week[dayOfWeek] = Constants.EmptyInlineKeyboardButton!;
                        continue;
                    }

                    week[dayOfWeek] = InlineKeyboardButton.WithCallbackData(
                        text: dayOfMonth.ToString(),
                        callbackData: $"{Constants.FullCommands.SetDate} {currentDate.ToShortDateString()}"
                    );

                    dayOfMonth++;
                }
                return week;
            }
        }

        InlineKeyboardButton[] GetButtons(DateTime currentDate) =>
            new[] {
                InlineKeyboardButton.WithCallbackData("<", $"{Constants.FullCommands.MonthBack} {currentDate.ToShortDateString()}"),
                Constants.EmptyInlineKeyboardButton!,
                InlineKeyboardButton.WithCallbackData(">", $"{Constants.FullCommands.MonthForward} {currentDate.ToShortDateString()}")
            };

        var resultButtons = new List<InlineKeyboardButton[]>
        {
            GetHeader(currentDate),
            GetDaysOfWeek().ToArray()
        };
        resultButtons.AddRange(GetCalendar(currentDate, cultureInfo));
        resultButtons.Add(GetButtons(currentDate));
        return new InlineKeyboardMarkup(resultButtons);
    }

    private static class Constants
    {
        public static DateTime MinimalDate = new(1800, 01, 01);

        public const string EmptyInlineKeyboardButton = " ";

        public static string MainCommand => "/calendarpicker";

        public static class SubCommands
        {
            public const string GetYearsKeyboard = "yearskeyboard";
            public const string GetMonthsKeyboard = "monthskeyboard";
            public const string YearBack = "yearback";
            public const string YearForward = "yearforward";
            public const string SetYear = "setyear";
            public const string SetMonth = "setmonth";
            public const string MonthBack = "monthback";
            public const string MonthForward = "monthforward";
            public const string SetDate = "setdate";
        }

        public static class FullCommands
        {
            public static string GetYearsKeyboard => $"{MainCommand} {SubCommands.GetYearsKeyboard}";
            public static string GetMonthsKeyboard => $"{MainCommand} {SubCommands.GetMonthsKeyboard}";
            public static string YearBack => $"{MainCommand} {SubCommands.YearBack}";
            public static string YearForward => $"{MainCommand} {SubCommands.YearForward}";
            public static string SetYear => $"{MainCommand} {SubCommands.SetYear}";
            public static string SetMonth => $"{MainCommand} {SubCommands.SetMonth}";
            public static string MonthBack => $"{MainCommand} {SubCommands.MonthBack}";
            public static string MonthForward => $"{MainCommand} {SubCommands.MonthForward}";
            public static string SetDate => $"{MainCommand} {SubCommands.SetDate}";
        }

        public static Dictionary<DayOfWeek, ItemInfo> DaysOfWeek => new()
        {
            { DayOfWeek.Monday, new("Понедельник", "Пн") },
            { DayOfWeek.Tuesday, new("Вторник", "Вт") },
            { DayOfWeek.Wednesday, new("Среда", "Ср") },
            { DayOfWeek.Thursday, new("Четверг", "Чт") },
            { DayOfWeek.Friday, new("Пятница", "Пт") },
            { DayOfWeek.Saturday, new("Суббота", "Сб") },
            { DayOfWeek.Sunday, new("Воскресенье", "Вс") }
        };

        public static Dictionary<string, ItemInfo> Months => new()
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
    }

    private static class Helpers
    {
        public static int GetLastDayOfMonth(DateTime currentDate)
        {
            var tempTime = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = tempTime.AddMonths(1).AddDays(-1).Day;
            return lastDayOfMonth;
        }

        public static int GetFirstDayOfWeek(DateTime currentDate, CultureInfo cultureInfo)
        {
            var tempTime = new DateTime(currentDate.Year, currentDate.Month, 1);
            return (7 + (int)tempTime.DayOfWeek - (int)cultureInfo.DateTimeFormat.FirstDayOfWeek) % 7;
        }

        public static string GetMonthName(DateTime currentDate) =>
            Constants.Months[CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(currentDate.Month)].Name;

        public static string GetMonthName(int monthNumber) =>
            Constants.Months[CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(monthNumber)].Name;

        public static List<string> GetSubcommandAndArgument(string calendarPickerCommand)
        {
            var splittedCommand = calendarPickerCommand.Split(' ').ToList();
            splittedCommand.RemoveAt(0);
            return splittedCommand;
        }
    }

    private class ItemInfo
    {
        public ItemInfo(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }

        public string Name { get; }
        public string ShortName { get; }
    }

    /// <summary>
    /// Объект результата выполнения процессора обработки запроса Calendar picker control
    /// </summary>
    public class CalendarPickerProcessorResult
    {
        /// <summary>
        /// Тип результата
        /// </summary>
        public ProcessorResultType ResultType { get; init; }
        /// <summary>
        /// Либо клавиатура
        /// </summary>
        public InlineKeyboardMarkup? KeyboardMarkup { get; init; }
        /// <summary>
        /// Либо установленная дата
        /// </summary>
        public DateTime? TargetDate { get; init; }
    }

    /// <summary>
    /// Тип результата отдаваемого процессором контрола
    /// </summary>
    public enum ProcessorResultType
    {
        /// <summary>
        /// Выбранная дата
        /// </summary>
        Date = 0,
        /// <summary>
        /// Контрол для отображения
        /// </summary>
        KeyboardMarkup = 1
    }
}