# Dev Container for Docs Development

This repository contains documents that are available at <https://specification.ardalis.com/>

You can use the dev container configuration in this folder to build and run the app without needing to install any of its tools locally! You can use it in [GitHub Codespaces](https://github.com/features/codespaces) or the [VS Code Dev Containers extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers).

## Docs Hosting Environment

The docs are hosted on GitHub Pages. GitHub provides a [GitHub Pages Ruby Gem](https://github.com/github/pages-gem) to make it easy for us to run this locally. 

If you want to know more about the dependencies of the GitHub Pages Ruby Gem, they keep track of [Dependency versions](https://pages.github.com/versions/).

## GitHub Codespaces

You can use the dev container with GitHub codespaces. Since we have a dev container built, use the guidance in [Creating a custom dev container configuration](https://docs.github.com/en/codespaces/setting-up-your-project-for-codespaces/adding-a-dev-container-configuration/introduction-to-dev-containers#creating-a-custom-dev-container-configuration).
  
## VS Code Dev Containers

You can also follow these steps to open this container using the VS Code Dev Containers extension:

1. If this is your first time using a development container, please ensure your system meets the pre-reqs (i.e. have Docker installed) in the [getting started steps](https://aka.ms/vscode-remote/containers/getting-started).

2. Open a locally cloned copy of the code:

   - Clone this repository to your local filesystem.   
   - Press <kbd>F1</kbd> and select the **Dev Containers: Open Folder in Container...** command.
   - Select the docs folder in the cloned repo, wait for the container to start, and try things out!

## Learn More about Dev Containers

- [NimblePros YouTube: Run GitHub Pages Locally in a Dev Container](https://www.youtube.com/watch?v=JpLJi5JBmYM&t=5s)
- [NimblePros Blog: Run GitHub Pages Locally in a Dev Container](https://blog.nimblepros.com/blogs/github-pages-with-dev-containers/)
- [NimblePros Blog: Introduction to Dev Containers](https://blog.nimblepros.com/blogs/introduction-to-dev-containers/)
- [NimblePros Webinar: Dev Containers Unwrapped!](https://www.youtube.com/watch?v=Wvetp2YkwPY)
