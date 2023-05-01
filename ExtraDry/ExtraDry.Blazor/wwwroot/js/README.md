The Bundles from bundleconfig.json are directed here (/js) instead of /bundles.

The bundled modules must be present in the project during CI for the Blazor framework to properly server them as '_content' files.

They are placed here to prevent any exclusion of them from occurring in .gitignore.
