# BlurbDB — Multilingual Content & Translation Management System

BlurbDB is an ASP.NET MVC 4 (C#, .NET Framework 4.5) web application for managing and
localizing short product/marketing text snippets ("blurbs") across multiple products,
categories, and languages. It gives translators and content editors a database-backed
UI to view, edit, import, and export localized strings, and it syncs approved content
between development and live/production databases via SQL Server / SSIS.

Originally built for managing product copy that needed translation into several
languages (Indonesian, Chinese, Russian, Spanish, and others), BlurbDB is a general
example of a **localization / i18n string database** and **translation management
system (TMS)** built on the classic ASP.NET MVC + Entity Framework stack.

![Screenshot](https://raw.github.com/DavidVeksler/BlurbDB/master/Documentation/Screenshot1.png)

![DB Schema](https://raw.github.com/DavidVeksler/BlurbDB/master/Documentation/DB%20Schema.png)

## Features

- **Blurb management UI** — browse, search, and edit text "blurbs" organized by
  product and category/area, built with Knockout.js, jQuery UI, and Bootstrap.
- **Multi-language translations** — each blurb can have one translation per culture
  (e.g. `es-ES`, `zh-CN`, `ru-RU`, `id-ID`), modeled with an Entity Framework
  (Database/Model-First, EDMX) schema (`Blurb`, `Category`, `Culture`,
  `Translation`, `Product`, `ProductCulture`).
- **Excel import/export** — bulk export blurbs and their translations to `.xlsx`
  for offline translation work, and re-import completed translations, using EPPlus
  (`OfficeOpenXml`).
- **Dev → Live sync** — stored procedures and SSIS packages
  (`SyncBlurbsDevSSIS` / `SyncBlurbsLiveSSIS`, `App_Data/SSIS/*.dtsx`) to promote
  translated content from a development database to production.
- **Web API endpoints** — `BlurbsController`, `TranslationController`,
  `ProductController`, `CategoryController`, `SearchController` expose blurb and
  translation data as JSON for the client-side app.

## Tech stack

- C# / ASP.NET MVC 4 (.NET Framework 4.5)
- Entity Framework 5 (Database-First / EDMX)
- Microsoft SQL Server
- SQL Server Integration Services (SSIS) for dev/live sync
- EPPlus for Excel import/export
- Knockout.js, jQuery, jQuery UI, Bootstrap on the front end
- Ninject for dependency injection

## Project layout

| Project | Purpose |
|---|---|
| `E1Blurbs.Data` | EF entity classes generated from the EDMX model (`Blurb`, `Category`, `Culture`, `Product`, `Translation`, ...) |
| `E1Blurbs.DataAccess` | Repositories and unit-of-work over the EF context |
| `E1Blurbs.Domain` | Business logic, including Excel import/export (`ImportExport/BlurbImporter.cs`, `BlurbExporter.cs`) |
| `E1Blurbs.Web.UI` | ASP.NET MVC web app: controllers, views, Web API endpoints, and static assets |
| `E1Blurbs.Web.UI.Tests` | Unit tests |
| `Create_DB_Script.sql` | Full SQL Server schema creation script |
| `SSIS_Sync_Sproc_Script.sql` | Stored procedures used by the dev/live SSIS sync jobs |

## Getting started

1. **Database**: Run `Create_DB_Script.sql` against a SQL Server instance to create
   the `BlurbDB` database and schema.
2. **Connection string**: Update the connection strings in
   `E1Blurbs.Web.UI/Web.config` (and the other projects' `App.config` files) to
   point at your SQL Server instance.
3. **Build**: Open `E1Blurbs.Web.UI.sln` in Visual Studio, restore NuGet packages,
   and build. This is a classic (non-.NET-Core) ASP.NET MVC 4 solution targeting
   .NET Framework 4.5.
4. **Run**: Launch `E1Blurbs.Web.UI` (e.g. via IIS Express) to use the web UI.
5. **Sync (optional)**: `SSIS_Sync_Sproc_Script.sql` and the packages in
   `E1Blurbs.Web.UI/App_Data/SSIS/` are used to sync blurb content between
   dev and live databases — see
   [Sync Data Between Instances of a Package (SSIS)](http://msdn.microsoft.com/en-us/library/ms136090.aspx).

## Status

This is an older project (originally released in 2013) shared as a reference
implementation of a database-driven localization/translation workflow on the
ASP.NET MVC + Entity Framework stack. It is not under active development.

## License

MIT — see [LICENSE](LICENSE).
