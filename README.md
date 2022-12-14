# CollageServices

This project was created so prevent having to query Google Photos while building a collage on my local server.  I use it to download 1024 x h thumbnails to facilitate building collages and mosaics while offline.

It tracks the Google Photos `id` so I can eventually download the highest quality visible segments of the photos I use.

It is also used to presist audio recording and notes.

## Roadmap

I do not expect this to have a very long future as I think it may be best to simply integrate with Google Photos directly.  I mostly created this to learn dotnet 7.

## Up Next

* pull and build svelte-lab locally
* pull missing images from google photos (1024 wide, max size)

## Other Notes

>dotnet ef dbcontext scaffold "Data Source=../photo.sqlite" Microsoft.EntityFrameworkCore.Sqlite -o Test -c PhotoContext
>dotnet user-secrets init
>dotnet user-secrets set ConnectionStrings:PhotoDatabase "Data Source=photo.sqlite"
>dotnet ef dbcontext scaffold Name=ConnectionStrings:PhotoDatabase Microsoft.EntityFrameworkCore.Sqlite -o Test -c PhotoContext
>"Name=ConnectionStrings:PhotoDatabase"

    System.InvalidOperationException: A named connection string was used, but the name 'ConnectionStrings:PhotoDatabase' was not found in the application's configuration. Note that named connection strings are only supported when using 'IConfiguration' and a service provider, such as in a typical ASP.NET Core application. See https://go.microsoft.com/fwlink/?linkid=850912 for more information.

### 2022.12.15

Today I learned how to build migrations to perform database upgrades, found #dotnet `aspnet-codegenerator` and generated views and controllers from #entityframework contexts, added #razor pages for simple crud ops and static pages to serve the #svelte based collage builder.
Although I was not thrilled with #blazor, #razor has strong appeal.
