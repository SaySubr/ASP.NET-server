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
        /// ������� ��������� � middleware
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_0(ref WebApplication app)
        {
            // ������� ������������\����������� middleware:
            app.MapGet("/", () => "Hello World!");        // ����� ���������� �������� HTTP GET
            app.MapGet("/test", () => "Hello Test!");     // ����� ���������� �������� HTTP GET
            app.MapGet("/admin", () => "Hello Admin!");   // ����� ���������� �������� HTTP GET

            // Middleware - ���������, �������������� ��� �������
            //app.UseWelcomePage(); // ���� ������������������, �� MapGet �� ������
            app.UseCookiePolicy();  // �� �������� ������� ���������� MapGet
        }

        /// <summary>
        /// ������������ Middleware
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_1(ref WebApplication app)
        {
            // �������������� ������������ middleware - ��������� �������� ��������� ��������,
            // ������� �� �������� ��������� middleware � �������  
            // - ������ ������ ����� ��������� ���� ������������ middleware: /test, /admin � �.�. 
            // - ����� ������ ����� ���� middleware �� ��������� 

            // ������ 1
            //app.Run(
            //    async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    }
            //);

            //������ 2 - �������� ��� ������������
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.StatusCode = 503;
            //        await context.Response.WriteAsync("The server is in service.");
            //    }
            //);

            // ������ 3 -  ��������� ���� middleware. 
            // ���������� middleware ��������� ���� ��� � ���������� �� ���������� ����� ���������� �����
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
        /// �������� ������� - HttpResponse
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_2(ref WebApplication app)
        {
            // ��� middleware �������� ������� ����� 
            // ������ HttpContext. ���� ������ ������������� 
            // ���������� � �������, ��������� ��������� ������� 
            // � ������ ������ 

            // ������ 1 - ������� �������� ������ 
            //app.Run(async context =>
            //    await context.Response.WriteAsync("Example of simple Response!")
            //    //await context.Response.WriteAsync("������!", System.Text.Encoding.Default); - �� �� �� ������ 
            //);

            // ������ 2 - ������� ���������
            //app.Run(async context =>
            //{
            //    var response = context.Response;
            //    response.Headers.ContentLanguage = "ru-RU";
            //    response.Headers.ContentType = "text/plain; charset=utf-8";
            //    response.Headers.Append("my-key", "-1");

            //    await response.WriteAsync("������ ���!");
            //}
            //);

            // ������ 3 - ��������� ���� �������
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.StatusCode = 404; // 503 - The server is in service.
            //        await context.Response.WriteAsync("Resource not Found!");
            //    }    
            //);

            // ������ 4 - �������� html-����
            app.Run(
                async context =>
                {
                    var response = context.Response;
                    response.ContentType = "text/html; charset=utf-8";
                    await response.WriteAsync(
                        "<h2>������ �4</h2>" +
                        "<p>��������� �����</p>"
                        );
                }
            );
        }


        private static void Sample_3(ref WebApplication app)
        {
            ////������ 1 - ��������� ���������� �������
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

            //������ 2 - �������� ���� ������� 
            // app.Run (async context => await context.Response.WriteAsync($"Path: {context.Request.Path}"));

            //������ 3 - ���������� �� ������ ����������/��������� ����
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

            //������ 4 - ������ ������.�� http request
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

            //������ 5 - ��������� ������� ���������� �������
            app.Run(
                async context =>
                {
                    var responce = context.Response;
                    var requsest = context.Request;
                    responce.ContentType = "text/html; charset = utf-8";

                    StringBuilder stringBuilder = new("<h3>��������� ������ �������</h3>");
                    stringBuilder.Append("<table>");
                    stringBuilder.Append("<tr><td>��������</td><td>��������</td></tr>");
                    //������ 1 - ����� ������� ���� ����������
                    foreach (var param in requsest.Query)
                    {
                        stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
                    }

                    stringBuilder.Append("</table>");


                    //������ 2 - �������� ��������� ����������
                   

                    

                }
            );
        }


        //�������� ������
        private static void Sample_4(ref WebApplication app)
        {
            //�������� �����
            //app.Run(
            //    async context =>
            //    {
            //        await context.Response.SendFileAsync("C:\\HTMLCSS\\AspNet\\Sample_0_\\Sample_ASP_NET_Core\\9f2bd726bf3c83210fba6d4a8ab449f9.jpg");
            //    }
            //);

            //�������� html ��������
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.ContentType = "text/html; charset=utf-8";
            //        await context.Response.SendFileAsync("C:\\HTMLCSS\\AspNet\\WebApplication1\\WebApplication1\\htmlpage.html");
            //    }
            //);

            //������ 3 - ��������� ����
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

            //������ 4 - �������� ����� � ������� �� �������
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

        //�������� ����
        private static void Sample_5(ref WebApplication app)
        {
            //������ 1 - ��������� ����� �����
            app.Run(
                async context =>
                {
                    var responce = context.Response;
                    var request = context.Request;

                    responce.ContentType = "text/html; chaset=utf-8";

                    if (request.Path == "/" || request.Path == "/login")//������� �� �������� �����������
                    {
                        await responce.SendFileAsync("html/login.html");
                    }
                    else if (request.Path == "/post_user_login")//������� �� ������� ��������
                    {
                        var form = request.Form;
                        string login = form["login"];
                        string password = form["password"];

                        if (login == "login" && password == "1234")//������� �� �������� ���������� � ����
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
