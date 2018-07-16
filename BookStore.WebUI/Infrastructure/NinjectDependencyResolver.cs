using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject; //3 
using Moq;
using BookStore.Domain.Abstract; //10
using BookStore.Domain.Entities;
using BookStore.Domain.Concrete;
using System.Configuration;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Infrastructure.Concrete;

namespace BookStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver //1,2
    {
        private IKernel kernel; //3
        public NinjectDependencyResolver(IKernel kernelParam) //5 "Constructor"
        {
            kernel = kernelParam;
            AddBindings(); //6 "Function"
        }


        public object GetService(Type serviceType)//2
        {
            return kernel.TryGet(serviceType); //4
        }

        public IEnumerable<object> GetServices(Type serviceType)//2
        {
            return kernel.GetAll(serviceType); //4
        }

        private void AddBindings() //6
        {
            //Mock<IBookRepository> mock = new Mock<IBookRepository>(); //10
            //mock.Setup(b => b.Books).Returns(
            //    new List<Book> { new Book {Title = "SQL Server DB", Price = 50M },
            //                     new Book {Title = "ASP.NET MVC 5", Price = 90M },
            //                     new Book {Title = "Web Client", Price = 87M } }
            //    );
            //kernel.Bind<IBookRepository>().ToConstant(mock.Object);

            EmailSettings emailSettings = new EmailSettings //143
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IBookRepository>().To<EFBookRepository>(); //25
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>() //144
                .WithConstructorArgument("setting", emailSettings);
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>(); //163
        }
    }
}