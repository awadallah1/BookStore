using System;
using System.Collections.Generic;
using Ninject;
using System.Web.Mvc;
using BookStore.Domain.Concrete;
using System.Configuration;
using BookStore.WebUI.Infrastructur.Abstract;
using BookStore.WebUI.Infrastructur.Concrete;
namespace BookStore.WebUI.Infrastructur
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelparam)
        {
            kernel = kernelparam;
            AddBindings();
        }

        private void AddBindings()
        {
            //Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            //mock.Setup(m => m.Books).Returns(
            //    new List<Book>
            //    {
            //        new Book {Title="asp",Price=30M },
            //        new Book {Title="MVC5",Price=350M }

            //    }
            //    );
            //kernel.Bind<IBooksRepository>().ToConstant(mock.Object);

            //putt initial emailsetting in web.config
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile=bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"]??"false")

            };

            kernel.Bind<IBooksRepository>().To<EFBooksRepository>();
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}