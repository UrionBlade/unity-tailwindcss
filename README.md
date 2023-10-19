<a name="readme-top"></a>


[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/UrionBlade/unity-tailwindcss">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">Unity Tailwind CSS</h3>

  <p align="center">
    A Unity package for using Tailwind CSS in Unity UI Toolkit
    <br />
    <a href="https://github.com/UrionBlade/unity-tailwindcss"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/UrionBlade/unity-tailwindcss">View Demo</a>
    ·
    <a href="https://github.com/UrionBlade/unity-tailwindcss/issues">Report Bug</a>
    ·
    <a href="https://github.com/UrionBlade/unity-tailwindcss/issues">Request Feature</a>
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
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

A Unity library that let user style Unity UI Toolkit elements using Tailwind CSS.
This library let users create a file to customize the default classes from Tailwind CSS and add new ones. The library will then generate a stylesheet that can be used in Unity UI Toolkit.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

* [![Tailwindcss][Tailwindcss]][Tailwindcss-url]
* [![C#][Csharp]][Csharp-url]


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple example steps.

### Prerequisites

You should have a Unity project with UI Toolkit installed.

### Installation

1. Open the Unity Package Manager
2. Press on the `+` button on the top left corner
3. Select `Add package from git URL...`
4. Insert `git@github.com:UrionBlade/unity-tailwindcss.git` and press `Add`
5. Once the package is installed create a TailwindGeneratorSettings asset by right clicking in the project window and selecting `Create > Tailwind Generator Settings`
6. From the generated asset inspector, click the button `Generate` to generate the stylesheet
7. To provide a personal configuration just create a json file (with the same structure of the tailwind one) and put it in a folder inside your project. Then drag the folder in the `Tailwind Generator Settings` inspector. The generator will use the configuration file to generate the stylesheet.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP
## Roadmap

- [ ] Feature 1
- [ ] Feature 2
- [ ] Feature 3
    - [ ] Nested Feature

See the [open issues](https://github.com/UrionBlade/unity-tailwindcss/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>
 -->

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Adds some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See [`LICENSE.txt`][license-url] for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Matteo Poli - [@MatteoPoli15](https://twitter.com/MatteoPoli15) - matteo.poli4@gmail.com

Project Link: [https://github.com/UrionBlade/unity-tailwindcss](https://github.com/UrionBlade/unity-tailwindcss)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/UrionBlade/unity-tailwindcss.svg?style=for-the-badge
[contributors-url]: https://github.com/UrionBlade/unity-tailwindcss/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/UrionBlade/unity-tailwindcss.svg?style=for-the-badge
[forks-url]: https://github.com/UrionBlade/unity-tailwindcss/network/members
[stars-shield]: https://img.shields.io/github/stars/UrionBlade/unity-tailwindcss.svg?style=for-the-badge
[stars-url]: https://github.com/UrionBlade/unity-tailwindcss/stargazers
[issues-shield]: https://img.shields.io/github/issues/UrionBlade/unity-tailwindcss.svg?style=for-the-badge
[issues-url]: https://github.com/UrionBlade/unity-tailwindcss/issues
[license-shield]: https://img.shields.io/github/license/UrionBlade/unity-tailwindcss.svg?style=for-the-badge
[license-url]: https://github.com/UrionBlade/unity-tailwindcss/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/matteo-poli-nft-factory
[product-screenshot]: images/screenshot.png
[Tailwindcss]: https://img.shields.io/badge/tailwindcss-06B6D4?style=for-the-badge&logo=tailwindcss&logoColor=white
[Tailwindcss-url]: https://tailwindcss.com/
[Csharp]: https://img.shields.io/badge/csharp-512BD4?style=for-the-badge&logo=csharp&logoColor=white
[Csharp-url]: https://learn.microsoft.com/en-us/dotnet/csharp/