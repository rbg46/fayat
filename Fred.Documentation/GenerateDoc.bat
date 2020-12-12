@echo off
:prérequis
: 1 Installer chocolatey
: 3 Installer Asciidoctor via ruby
: soit 
: Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
: choco install ruby
: gem install asciidoctor
:  
: 4 Installer Asciidoctor diagram via ruby
: gem install asciidoctor-diagram
:  
: WARNING : il est possible qu'il faille installer graphviz ici si erreur https://graphviz.gitlab.io/_pages/Download/Download_windows.html
:  
: pour convertir du md en asciidoc
: pandoc -o Readme.adoc Readme.md

asciidoctor -a data-uri  -a graphviz="C:\Program Files (x86)\Graphviz2.38\bin" -r asciidoctor-diagram -o Documentation.html .\Index.adoc