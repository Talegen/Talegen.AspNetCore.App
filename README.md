# Talegen.AspNetCore.App
Common Support Code for ASP.NET Core Applications Built by Talegen

## Introduction
This library is a collection of common support code for ASP.NET Core applications built by Talegen. This library is not intended to be used by other applications. This library is used by Talegen applications to provide common support code for ASP.NET Core applications.

## Installation
This library is available via NuGet. To install, use the following command - 

```powershell
Install-Package Talegen.AspNetCore.App
```

## Usage

This library is used by Talegen applications to provide common support code for ASP.NET Core applications. This library is not intended to be used by other applications.

The main entry point of any ASP.net application is the [IHostBuilder] builder interface. This library is used to leverage the startup process
and extend controllers within an MVC-based ASP.net Core application, with commonly utilized services and support classes for best-practices.

### Startup

The [Startup] class is the main entry point for any ASP.net Core application. This class is used to configure the application services and middleware.
This library provides a base class [Talegen.AspNetCore.App.Startup] that can be used to extend the [Startup] class. This base class provides a common set of services and middleware that are commonly used in Talegen applications.

### Controllers

The [Talegen.AspNetCore.App.Controllers] namespace contains a base class [Talegen.AspNetCore.App.Controllers.ControllerBase] that can be used to extend the [ControllerBase] class. This base class provides a common set of services and middleware that are commonly used in Talegen applications.

### Services

The [Talegen.AspNetCore.App.Services] namespace contains a base class [Talegen.AspNetCore.App.Services.ServiceBase] that can be used to extend the [ServiceBase] class. This base class provides a common set of services and middleware that are commonly used in Talegen applications.

### Models

The [Talegen.AspNetCore.App.Models] namespace contains a base class [Talegen.AspNetCore.App.Models.ModelBase] that can be used to extend the [ModelBase] class. This base class provides a common set of services and middleware that are commonly used in Talegen applications.

### Repositories

The [Talegen.AspNetCore.App.Repository] namespace contains a base class [Talegen.AspNetCore.App.Repositories.RepositoryBase] that can be used to extend the [RepositoryBase] class. This base class provides a common set of services and middleware that are commonly used in Talegen applications.


## License
This library is licensed under the [Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0) license.

## Change Log
1.0.0 - Initial release of the library.

## Feedback and Documentation
Feedback and Documentation can be found at [Issues](https://github.com/Talegen/Talegen.AspNetCore.App/issues).
