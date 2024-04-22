# new-umbraco-vite

Very simple project to ease adding frontend building to a fresh Umbraco site.

## Usage

Clone this project using f ex degit to the root of your Umbraco site.

`degit jannemartensgard/new-umbraco-vite build`

Add the `Microsoft.AspNetCore.SpaServices.Extensions` to your Umbraco project.

Move the `cs-files/HelperService.cs` file to a suitable folder in your Umbraco installation. Adjust the namespace to fit.

Add the following code to the `<head>` section of your `_Layout.cshtml` file:

```csharp
@inject IHelpersService Helpers
@{
    var resources = Helpers.GetResourcesFromManifest();
}
<environment names="Development">
    <script type="module" src="~/dist/@@vite/client"></script>
    <script type="module" src="~/dist/js/index.ts"></script>
</environment>
<environment exclude="Development">
    @if (resources != null)
    {
        foreach (var cssItem in resources)
        {
            if (cssItem.Css != null)
            {
                foreach (var cssFile in cssItem.Css)
                {
                    <link rel="stylesheet" href="~/dist/@(cssFile)"/>
                }
            }
        }
    }
</environment>
```

Add the following code to either the Startup.cs file or the Program.cs file depending on Umbraco version:

```csharp
if (_env.IsDevelopment())
{
    app.Map("/dist", innerApp =>
    {
        innerApp.UseSpa(spa =>
        {
            spa.Options.SourcePath = "src";
            spa.Options.DevServerPort = 3000;
            spa.UseReactDevelopmentServer(npmScript: "start");
        });
    });
}
```

### Umbraco 12

Add the code to the Configure method in Startup.cs:

```csharp
... 

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Add code here
    ...
}
```

### Umbraco 13

Add the following code to Program.cs: 

```csharp
...

WebApplication app = builder.Build();

// Add code here

await app.BootUmbracoAsync();

...
```

Restart your Umbraco project and it should now automatically build front end files and hot reload when necessary.
