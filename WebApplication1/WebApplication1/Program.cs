using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            Sample_5(ref app);

            app.Run();
        }

        /// <summary>
        /// Конвеер обработки и middleware
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_0(ref WebApplication app)
        {
            // Примеры использвания\подключения middleware:
            app.MapGet("/", () => "Hello World!");        // метод расширения запросов HTTP GET
            app.MapGet("/test", () => "Hello Test!");     // метод расширения запросов HTTP GET
            app.MapGet("/admin", () => "Hello Admin!");   // метод расширения запросов HTTP GET

            // Middleware - компонент, обрабатывающий все запросы
            //app.UseWelcomePage(); // если расскамментировать, то MapGet не увидем
            app.UseCookiePolicy();  // не помешает увидеть зареганные MapGet
        }

        /// <summary>
        /// Терминальные Middleware
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_1(ref WebApplication app)
        {
            // Зарегистрируем терминальный middleware - компонент конвеера обработки запросов,
            // который не вызывает следующий middleware в цепочки  
            // - каждый запрос будет обработан этим терминальным middleware: /test, /admin и т.д. 
            // - людой другой после него middleware не сработает 

            // Пример 1
            //app.Run(
            //    async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    }
            //);

            //Пример 2 - заглужка тех обслуживания
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.StatusCode = 503;
            //        await context.Response.WriteAsync("The server is in service.");
            //    }
            //);

            // Пример 3 -  жизненный цикл middleware. 
            // Компоненты middleware создаются один раз и существуют на протяжении всего жизненного цикла
            int x = 2;
            app.Run(
                async context =>
                {
                    x *= 2;
                    await context.Response.WriteAsync($"Result = {x}");
                }
            );
        }

        /// <summary>
        /// Отправка ответов - HttpResponse
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_2(ref WebApplication app)
        {
            // Все middleware передают запросы через 
            // объект HttpContext. Этот объект инкапсулирует 
            // информацию о запросе, позволяет управлять ответом 
            // и многое другое 

            // Пример 1 - простая отправка ответа 
            //app.Run(async context =>
            //    await context.Response.WriteAsync("Example of simple Response!")
            //    //await context.Response.WriteAsync("Пример!", System.Text.Encoding.Default); - на РУ не пойдет 
            //);

            // Пример 2 - уставка заголовка
            //app.Run(async context =>
            //{
            //    var response = context.Response;
            //    response.Headers.ContentLanguage = "ru-RU";
            //    response.Headers.ContentType = "text/plain; charset=utf-8";
            //    response.Headers.Append("my-key", "-1");

            //    await response.WriteAsync("Привет мир!");
            //}
            //);

            // Пример 3 - установка кода статуса
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.StatusCode = 404; // 503 - The server is in service.
            //        await context.Response.WriteAsync("Resource not Found!");
            //    }    
            //);

            // Пример 4 - отправка html-кода
            app.Run(
                async context =>
                {
                    var response = context.Response;
                    response.ContentType = "text/html; charset=utf-8";
                    await response.WriteAsync(
                        "<h2>Пример №4</h2>" +
                        "<p>Некоторый текст</p>"
                        );
                }
            );
        }


        private static void Sample_3(ref WebApplication app)
        {
            ////пример 1 - получение заголовков запроса
            //app.Run
            //    (
            //    async context =>
            //    {
            //        context.Response.ContentType = "text/html; charse=utf-8"
            //        StringBuilder stringBuilder = new("<table>");

            //        foreach (var header in context.Request.Readers)
            //        {
            //            stringBuilder.Append($"<tr><td>{header.key}</td><tr><td>{header.Value}</td>")
            //            }
            //        stringBuilder.Append("/table");

            //        await context.Response.WriteAsync(stringBuilder.ToString());
            //    }
            //    );

            //пример 2 - Получить путь запроса 
            // app.Run (async context => await context.Response.WriteAsync($"Path: {context.Request.Path}"));

            //Пример 3 - фильтрация на основе ведденного/Указаного пути
            //app.Run(
            //    async context =>
            //    {
            //        var path = context.Request.Path
            //        var Date_now = DateTime.Now;
            //        var responce = context.Response;

            //        if (path == "/date")
            //            await responce.WriteAsync($"Date:{Date_now.ToShortDateString()}");
            //        else if (path == "/time")
            //            await responce.WriteAsync($"Time:{Date_now.ToShortTimeString()}");
            //        else if (path == "/time/date")
            //            await responce.WriteAsync($"Date:{Date_now.ToShortDateString()}\n\r" +
            //                $"Time:{Date_now.ToShortTimeString()}");
            //        else
            //            await responce.WriteAsync($"HHello WWoorrlldd!");
            //    }
            //);

            //Пример 4 - строка запрос.из http request
            //app.Run
            // (
            //    async context =>
            //    {
            //        var responce = context.Response;
            //        responce.ContentType = "text/html; charset = utf-8";
            //        await responce.WriteAsync(
            //            $"<p>path:{context.Request.Path}</p>" +
            //            $"<p>QueryString:{context.Request.QueryString}</p>" +
            //            );
            //    }
            // );

            //пример 5 - получения словаря параметров запроса
            app.Run(
                async context =>
                {
                    var responce = context.Response;
                    var requsest = context.Request;
                    responce.ContentType = "text/html; charset = utf-8";

                    StringBuilder stringBuilder = new("<h3>Параметры строки запроса</h3>");
                    stringBuilder.Append("<table>");
                    stringBuilder.Append("<tr><td>Параметр</td><td>значение</td></tr>");
                    //пример 1 - берем колецию всех параметров
                    foreach (var param in requsest.Query)
                    {
                        stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
                    }

                    stringBuilder.Append("</table>");


                    //Пример 2 - значение отдельных параметров
                   

                    

                }
            );
        }


        //Отправка файлов
        private static void Sample_4(ref WebApplication app)
        {
            //Отправка файла
            //app.Run(
            //    async context =>
            //    {
            //        await context.Response.SendFileAsync("C:\\HTMLCSS\\AspNet\\Sample_0_\\Sample_ASP_NET_Core\\9f2bd726bf3c83210fba6d4a8ab449f9.jpg");
            //    }
            //);

            //Отправка html страницы
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.ContentType = "text/html; charset=utf-8";
            //        await context.Response.SendFileAsync("C:\\HTMLCSS\\AspNet\\WebApplication1\\WebApplication1\\htmlpage.html");
            //    }
            //);

            //Пример 3 - обработка пути
            //app.Run(
            //    async context=>
            //    {
            //        var path = context.Request.Path;
            //        var FullPath = $"html/{path}.html";
            //        var responce = context.Response;

            //        responce.ContentType = "Text/html; chatset=utf-8";

            //        if (File.Exists(FullPath))
            //        {
            //            await responce.SendFileAsync(FullPath);
            //        }
            //        else
            //        {
            //            responce.StatusCode = 404;
            //            await responce.WriteAsync("<h2>Not Found</h2>");
            //        }
            //    }
            //);

            //пример 4 - загрузка файла с сервера на клиента
            app.Run(
                async context =>
                {
                    var path = context.Request.Path;
                    if (path == "/domnload")
                    {
                        context.Response.Headers.ContentDisposition = "attachment; filename = tree.jpeg";
                        await context.Response.SendFileAsync("135ac7c239592d0a8b8d5589e625a826.jpeg");
                    }
 
                }
            );
        }

        //отправка форм
        private static void Sample_5(ref WebApplication app)
        {
            //пример 1 - обработка формы ввода
            app.Run(
                async context =>
                {
                    var responce = context.Response;
                    var request = context.Request;

                    responce.ContentType = "text/html; chaset=utf-8";

                    if (request.Path == "/" || request.Path == "/login")//переход на страницу авторизации
                    {
                        await responce.SendFileAsync("html/login.html");
                    }
                    else if (request.Path == "/post_user_login")//переход на главную страницу
                    {
                        var form = request.Form;
                        string login = form["login"];
                        string password = form["password"];

                        if (login == "login" && password == "1234")//переход на страницу информации о себе
                        {
                            await responce.SendFileAsync("html/Main.html");
                        }
                        else
                        {
                            await responce.WriteAsync($"<div><h3>{login} - {password}: Not Found!</h3></div>");
                        }
                    }
                    else if (request.Path == "/post_user")
                    {
                        var form = request.Form;

                        string name = form["name"];
                        string age = form["age"];

                        string[] lanquages = form["lanquegesges"];

                        string langstr = "";
                        foreach(var str in lanquages)
                        {
                            langstr += str + " ";
                        }
                        await responce.WriteAsync($"<div" +
                            $"<p>Name:{name}</p>" +
                            $"<p>Name:{age}</p>" +
                            $"<p>Languages:{langstr}</p>" +
                            $"</div"
                           );
                    }

                }            
            );
        }


    }
}
