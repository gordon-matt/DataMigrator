[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=gordon_matt%40live%2ecom&lc=AU&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted)

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Logo.png" alt="Logo" width="250" />

# Data Migrator

## Intro

This was an interesting project I started some years ago. The idea is that you should be able to migrate from any database system to any database system... and it's extensible via plugins (developed with MEF). Development on this was cancelled due to no longer being needed at the time. I have now "dusted it off" so to speak and fixed many bugs that I found, improved the UI, added features, updated the target framework to .NET 7, and updated NuGet packages, etc. I have also tested the SQL Server, MySQL, PostgreSQL and delimited file migrations and it seems to be working quite well.

There are currently plugins for the following systems:

- [x] **SQL Server** (Working)
- [x] **PostgreSQL** (Working)
- [x] **MySQL/MariaDB** (Working)
- [x] **CSV** (Working)
- [ ] **Access** (Needs further development to support .NET 7)
- [ ] **SharePoint** (Untested. Developed this years ago. The APIs seem to have remained the same. Needs testing when I find time).

## Screenshots

**Startup**:

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshots/Startup.PNG" alt="Startup" />

**Connections**:

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshots/Connections.PNG" alt="Connections" />

**Adding a new "job"** (essentially, a job = a table to be transferred):

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshots/NewJob.png" alt="NewJob" />

**"Add Job" dialog:**

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshots/AddJob.PNG" alt="AddJob" />

**The Table Mappings screen**
This screen lets you match source to destination via the buttons on the bottom of the screen. It is divided into 3 sections:
  - **Fields of source table** shown on left
  - **Fields of destination table** shown on right.
  - **Mapped fields** in the center

**Buttons** as follows:
  - **Create Destination Table**: As the name suggests, it will create a table in the destination based on the schema of the source table. 
  - **Add**: Maps the selected source and destination rows. Removes them from their respective grids and adds the row in the center grid.
  - **Remove**: Removes the selected mapping and restores the rows in the source and destination grids.
  - **Auto Map**: Will automatically map all fields it is able to (based on name and data type)
  - **Add/Edit Script**: Shows a popup window for writing a C# script to transform the data for a given mapping. For example: You may wish to transform "Yes" and "No" values to "1" and "0" for a boolean (BIT) destination column. See screenshot a little further down for an example of this...

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshots/TableMappings_1.PNG" alt="TableMappings_1" />

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshots/TableMappings_2.PNG" alt="TableMappings_2" />

**Script dialog:**

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshots/Script.PNG" alt="Script" />

**Settings** (You can specify batch size and whether or not to trim strings from source before insert to destination):

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshots/Settings.PNG" alt="Settings" />

**Runing job:**

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshots/Running.PNG" alt="Running" />

## License

This project is licensed under the [MIT license](LICENSE.txt).

## Support

If you find this project useful, consider supporting its development.

<p align="center">
  <a href="https://buymeacoffee.com/vnmatt">
    <img src="https://github.com/gordon-matt/MyResume/blob/gh-pages/assets/images/BuyMeACoffee_Logo.png"
         alt="Buy Me a Coffee"
         height="40">
  </a>&nbsp;&nbsp;&nbsp;
  <a href="https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=gordon_matt%40live%2ecom&lc=AU&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted">
    <img src="https://github.com/gordon-matt/MyResume/blob/gh-pages/assets/images/PayPal_Logo.png"
         alt="Donate with PayPal"
         height="40">
  </a>
</p>

<p align="center">
  Prefer using your phone? Scan the QR code:
</p>

<p align="center">
  <a href="https://buymeacoffee.com/vnmatt">
    <img src="https://github.com/gordon-matt/MyResume/blob/gh-pages/assets/images/BuyMeACoffee_QR.png"
         alt="Buy Me a Coffee QR Code"
         width="180">
  </a>
</p>

<img src="https://komarev.com/ghpvc/?username=gordon-matt&label=GitHub%20Hits%20Since%202025-06-01%3A%20&color=ff0000&style=flat" alt="gordon-matt" />
