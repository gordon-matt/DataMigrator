[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=gordon_matt%40live%2ecom&lc=AU&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted)

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Logo.png" alt="Logo" width="250" />

# Data Migrator

## Intro

This was an interesting project I started some years ago. The idea is that you should be able to migrate from any database system to any database system... and it's extensible via plugins (developed with MEF). There are currently plugins for the following database systems:

SQL Server
PostgreSQL
MySQL (also works on MariaDB)
Access
Excel
CSV
SharePoint
SQL Compact Edition

Note: Development on this project was cancelled due to no longer being needed at the time. However, most of the work was already done. I have now "dusted it off" so to speak and fixed a few bugs that I found, updated the target .NET Framework to 4.8, and updated NuGet packages, etc. I have also tested the SQL Server, MySQL and Postgres migrations and it seems to be working quite well. The project could definitely do with some thorough testing and polishing off though, as the UI is a little "buggy" in some places. For example: loading one saved file after having already opened another doesn't seem to reset things properly with the connections view.

I welcome any pull requests to fix bugs or add new features.

## Screenshots

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshot_01_Connections.PNG" alt="Connections" width="250" />

<img src="https://github.com/gordon-matt/DataMigrator/blob/master/_Misc/Screenshot_02_Job.PNG" alt="Job" width="250" />

## License

This project is licensed under the [MIT license](LICENSE.txt).

## Donate
If you find this project helpful, consider buying me a cup of coffee.  :-)

#### PayPal:

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=gordon_matt%40live%2ecom&lc=AU&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted)

#### Crypto:
- **Bitcoin**: 1EeDfbcqoEaz6bbcWsymwPbYv4uyEaZ3Lp
- **Ethereum**: 0x277552efd6ea9ca9052a249e781abf1719ea9414
- **Litecoin**: LRUP8hukWGXRrcPK6Tm7iUp9vPvnNNt3uz

