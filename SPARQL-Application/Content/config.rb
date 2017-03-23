require 'sass-globbing'
require 'singularitygs'
require 'breakpoint'
require 'fileutils'

project_type = :stand_alone
http_path = "/"
sass_dir = "scss"
css_dir = "css/dev"
images_dir = "img"
fonts_dir = "fonts"
javascripts_dir = "js"

line_comments = false
preferred_syntax = :scss
output_style = :expanded
relative_assets = true

on_stylesheet_saved do
  `compass compile -c config-prod.rb --force`
end