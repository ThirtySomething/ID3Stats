# MP3STATS

Various statistics based on ID3 tags of MP3 files.

## Motivation

I'm ripping all my CDs to MP3 files. With a number of > 800 CDs I've got a large number of MP3 files. After listening to a song I knew from another artist I thought about some statistics.

## Details

The program is splitted up into two parts. The `data collector` will collect the information from the MP3 files and will store the data into a database.

The second part `MP3Stats` will run several plugins. Each plugin will determine a statistic.

### Data collector

The `DataCollector` will work the following steps:

- Check if given path is existing, abort if not
- Find all MP3 files
- For each MP3 file
  - Read ID3 tag
  - Insert/Update ID3 meta data in database

For details see [readme][app_datacollector] of `datacollector` project. Currently there is no logic for check move of MP3 files inside the collection. But to detect this a filehash (MD5) is generated during import and written to the DB. This may slow down the process.

### MP3STATS

The `MP3Stats` program will establish a connection to the database. Additional an output stream for a HTML file is created. Then it will load all plugins and pass the db connection as well as the output stream to each plugin. Forces by an `interface` each plugin will run a simple db task, the results are written to the passed output stream.

For details see [readme][app_mp3stats] of `mp3stats` project.

### Plugin mechanism

The plugin mechanism is based on my other project, `weatherstation` [here][project_weatherstation].

### ERM diagram

![mp3stats ERM diagram](./images/mp3stats.png "mp3stats ERM diagram")

The source of the diagram is [here][file_erm]. To get fast results for the plugins a flat table is used. This will also simplify the queries in the plugins.

The diagram is made with [PlantUML][tool_puml].

## Technology

- This program is written in [C#][code_c#]
- Inspired from my other project [Weatherstation][project_weatherstation]
  - The configuration file handling
  - The plugin system
- Reading the ID3 tags is based on [TagLibSharp][lib_taglibsharp]

## Tasks

- Switch from MariaDB to SQLite

## Libraries

All used libraries are sticked to `mp3stats_core` to have no redundancy of various versions of the NuGet packages.

- [Microsoft.EntityFrameworkCore][lib_efc] - [MIT licence][licence_mit]
- [NLog][lib_nlog] - [BSD3 clause licence][licence_bsd3]
- [Newtonsoft.Json][lib_newton_json] - [MIT licence][licence_mit]
- [Pomelo.EntityFrameworkCore.MySql][lib_pomelo] - [MIT licence][licence_mit]
- [z440.atl.core][lib_taglibsharp], also known as [TagLib#][lib_taglibsharp] - [MIT licence]

[app_datacollector]: ./datacollector/README.md
[app_mp3stats]: ./mp3stats/README.md
[code_c#]: https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/
[file_erm]: ./mp3stats.puml
[lib_efc]: https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/
[lib_newton_json]: https://www.nuget.org/packages/Newtonsoft.Json/
[lib_nlog]: https://www.nuget.org/packages/NLog/
[lib_pomelo]: https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/
[lib_taglibsharp]: https://github.com/mono/taglib-sharp
[lib_taglibsharp]: https://www.nuget.org/packages/z440.atl.core/
[licence_bsd3]: https://licenses.nuget.org/BSD-3-Clause
[licence_mit]: https://licenses.nuget.org/MIT
[project_weatherstation]: https://github.com/ThirtySomething/Weatherstation
[tool_puml]: https://plantuml.com/
