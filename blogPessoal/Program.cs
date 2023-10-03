
using blogpessoal.Security;
using blogpessoal.Security.Implements;
using blogpessoal.Service.Implements;
using blogPessoal.Data;
using blogPessoal.Model;
using blogPessoal.Security;
using blogPessoal.Service;
using blogPessoal.Service.Implements;
using blogPessoal.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace blogPessoal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Controller Class
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

            // Add services to the container.

            builder.Services.AddControllers();

            //Conexão com o banco de dados.
            var conectionString = builder.Configuration
                .GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(conectionString));

            //Registrar a validacao das entidades
            builder.Services.AddTransient<IValidator<Postagem>, PostagemValidator>();
            builder.Services.AddTransient<IValidator<Tema>, TemaValidator>();
            builder.Services.AddTransient<IValidator<User>, UserValidator>();
            



            //Registrar as classes de Servico
            builder.Services.AddScoped<IPostagemService, PostagemService>();
            builder.Services.AddScoped<ITemaService, TemaService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddTransient<IAuthService, AuthService>();

            // Adicionar a Validação do Token JWT

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Settings.Secret);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });





            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Configuracao do CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            //Criar config necessaria para Gerar o Banco
            using (var scope = app.Services.CreateAsyncScope())
            {
                var dbContect = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContect.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Inicializa o Cors
            app.UseCors("MyPolice");


            // Habilitar a Autenticação e a Autorização
            app.UseAuthentication();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}