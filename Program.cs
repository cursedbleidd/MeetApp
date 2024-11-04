namespace MeetApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? choice = null;
            MeetupManager meetupManager = new MeetupManager(OnMeetupStartingSoon);
            while (choice != "0")
            {
                Console.WriteLine(
                    "0. Выход\n" +
                    "1. Добавить встречу\n" +
                    "2. Удлаить встречу\n" +
                    "3. Изменить встречу\n" +
                    "4. Расписание на день\n" +
                    "5. Расписание на день в файл"
                    );

                choice = Console.ReadLine();
                try
                {
                    switch (choice)
                    {
                        case "1":
                            {
                                Console.WriteLine("<Название> <Описание> <Начало> <Конец> <Уведомить за>:");
                                string[] line = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                                if (line.Length != 7)
                                    throw new Exception("Meetup requires 5 arguments");
                                meetupManager.AddMeetup(line[0], line[1], DateTime.Parse($"{line[2]} {line[3]}"), DateTime.Parse($"{line[4]} {line[5]}"), TimeSpan.FromMinutes(double.Parse(line[6])));
                                break;
                            }
                        case "2":
                            {
                                Console.WriteLine("Введите id:");
                                int id;
                                if (int.TryParse(Console.ReadLine(), out id))
                                    meetupManager.RemoveMeetup(id);
                                break;
                            }
                        case "3":
                            {
                                Console.WriteLine("Введите id:");
                                int id;
                                if (!int.TryParse(Console.ReadLine(), out id))
                                    break;
                                Console.WriteLine("<Название> <Описание> <Начало> <Конец> <Уведомить за>:");
                                string[] line = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                                if (line.Length != 7)
                                    throw new Exception("Meetup requires 5 arguments");
                                meetupManager.ChangeMeetup(id, line[0], line[1], DateTime.Parse($"{line[2]} {line[3]}"), DateTime.Parse($"{line[4]} {line[5]}"), TimeSpan.FromMinutes(double.Parse(line[6])));
                                break;
                            }
                        case "4":
                            Console.WriteLine("Введите дату:");
                            Console.WriteLine(meetupManager.GetStringMeetupsForDate(DateTime.Parse(Console.ReadLine())));
                            break;
                        case "5":
                            Console.WriteLine("Введите дату:");
                            string text = meetupManager.GetStringMeetupsForDate(DateTime.Parse(Console.ReadLine()));
                            Console.WriteLine("Введите имя файла:");
                            File.WriteAllText(Console.ReadLine(), text);
                            break;
                        default: break;
                    }
                }
                catch (Exception e){
                    Console.WriteLine(e.Message);
                    choice = null;
                }
            }
            Console.Read();
        }
        static void OnMeetupStartingSoon(object? sender, EventArgs e)
        {
            if (sender is Meetup meetup)
            {
                Console.WriteLine($"Внимание! Встреча '{meetup.Name}' начнется в {meetup.Start}! Описание: {meetup.Description}");
            }
        }
    }
}
