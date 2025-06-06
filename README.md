<a name="readme-top"></a>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/Adam-Gladstone/BookCatalog">
    <img src="BookCatalog/Assets/WindowIcon.ico" alt="logo" width="80" height="80">
  </a>

  <h3 align="center">BookCatalog (v1.1)</h3>

  <p align="center">
    <br />
    <a href="https://github.com/Adam-Gladstone/BookCatalog"><strong>Explore the docs >></strong></a>
    <br />
    <br />
    <a href="https://github.com/Adam-Gladstone/BookCatalog/issues">Report Bug</a>
    ·
    <a href="https://github.com/Adam-Gladstone/BookCatalog/issues">Request Feature</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project
I have around 500+ e-books (mostly `pdf`'s but also some `epub`'s). I wanted to be able to easily search for a specific title or author, as I don't always remember either exactly (e.g. *"erm, I think it was about 'compilers'; maybe the author was 'Holub' or 'Aho' ...")*. I have tried a number of approaches to this: Windows search, PowerShell script, and so on. None of these were really satisfactory for one reason or another. Eventually, I settled for writing a simple cataloguing application. This is it (or at least an initial version of it).

__BookCatalog__ is a Windows desktop application that allows you to list and organise your (electronic) book collection. It is simple to add titles to the collection. Just point to a directory and add files. The application provides simple facilities to search, sort, filter and group books. Once you have found the book(s) you were looking for, you can double-click to open the file in the associated application.

__BookCatalog__ is written in C# (.NET8.0) and uses WinUI and XAML for the user interface.

__BookCatalog__ demonstrates:
- C#/WinUI and XAML for the UI
- Local file-based database using SQLite
- Community Toolkit features, specifically
    - settings page (with support for light/dark themes)
	- page navigation
	- dependency injection
	- flyout menus
	- a local sqlite database
	- MVVM architecture
- Custom exception dialog
- Separate projects for the core (reusable functionality), testing, and user interface.
- Unit testing using NUnit

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

* Visual Studio 2022
* C# (.NET8.0)

The following packages are used:
* CommunityToolkit.Mvvm (8.3.2)
* CommunityToolkit.WinUI.Controls.SettingsControls (8.1.240916)
* CommunityToolkit.WinUI.Controls.Primitives (8.1.240916)
* CommunityToolkit.WinUI.UI.Controls.DataGrid (7.1.2)
* Microsoft.Extensions.Configuration (9.0.0)
* Microsoft.Extensions.Hosting (9.0.0)
* Microsoft.WindowsAppSDK (1.6.241114003)
* Microsoft.Xaml.Behaviors.WinUI.Managed (2.0.9)
* WinUIEx (2.5.0)
* Dapper (2.1.35")
* Dapper.Contrib (2.0.78)
* Microsoft.Data.Sqlite (9.0.0)
* Newtonsoft.Json (13.0.3)
* Microsoft.Graphics.Win2D (1.3.0)
* Microsoft.NET.Test.Sdk (17.12.0)
* NUnit (4.2.2)
* NUnit3TestAdapter (4.6.0)
* NUnit.Analyzers (4.4.0)
* coverlet.collector (6.0.2)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started
The project can be downloaded from the GitHub repository in the usual way.

The solution consists of three projects:
* BookCatalog: this contains the main application code.
* BookCatalog.Core: this contains the core reusable code. In this case it only consists of the book model class and the database service.
* BookCatalog.Tests.NUnit: this consists of unit tests for the core database functionality.

### Prerequisites

### Installation

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->
## Usage
Build and start the application for the first time. You will be presented with an empty Catalog screen. 

<a href="https://github.com/Adam-Gladstone/BookCatalog">
  <img src="Images/EmptyCatalog.png" alt="Initial Empty Catalog (BookCatalog)">
</a>

Locate and press the 'Add' button. The Open File Dialog is displayed. Navigate to the directory where the books are located. It is useful if the books are organised by a category as this can be used for grouping. Otherwise the titles are presented in a simple list. Select all the files to be added and press 'OK'. Wait a few seconds while the titles are being added to the database, then you are presented with a list.

<a href="https://github.com/Adam-Gladstone/BookCatalog">
  <img src="Images/UngroupedSearch.png" alt="Ungrouped Search (BookCatalog)">
</a>

The default is to list the books by title (alphabetically). The category button allows grouping by Category.

<a href="https://github.com/Adam-Gladstone/BookCatalog">
  <img src="Images/GroupedSearch.png" alt="Grouped Search (BookCatalog)">
</a>

<p></p>
The search bar can be used to filter the titles.

<a href="https://github.com/Adam-Gladstone/BookCatalog">
  <img src="Images/FilterTitle.png" alt="Filter (BookCatalog)">
</a>



<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->
## Roadmap

Future directions:

See the [open issues](https://github.com/Adam-Gladstone/BookCatalog/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->
## License

Distributed under the GPL-3.0 License. See `LICENSE.md` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTACT -->
## Contact

Adam Gladstone - (https://www.linkedin.com/in/adam-gladstone-b6458b156/)

Project Link: [https://github.com/Adam-Gladstone/BookCatalog](https://github.com/Adam-Gladstone/BookCatalog)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

Helpful resources

* [Choose an Open Source License](https://choosealicense.com)
* [GitHub Pages](https://pages.github.com)
* [Font Awesome](https://fontawesome.com)
* [React Icons](https://react-icons.github.io/react-icons/search)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- PROJECT SHIELDS -->

[![Issues][issues-shield]][issues-url]
[![GPL-3 License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[issues-shield]: https://img.shields.io/github/issues/Adam-Gladstone/BookCatalog.svg?style=for-the-badge
[issues-url]: https://github.com/Adam-Gladstone/BookCatalog/issues

[license-shield]: https://img.shields.io/github/license/Adam-Gladstone/BookCatalog.svg?style=for-the-badge
[license-url]: https://github.com/Adam-Gladstone/BookCatalog/LICENSE.md

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/adam-gladstone-b6458b156/
                      
<a name="readme-top"></a>

