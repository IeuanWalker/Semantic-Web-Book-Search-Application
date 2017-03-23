require 'sass-globbing'
require 'singularitygs'
require 'breakpoint'

project_type = :stand_alone
http_path = "/"
sass_dir = "scss"
css_dir = "css"
images_dir = "img"
fonts_dir = "fonts"
javascripts_dir = "js"

line_comments = false
preferred_syntax = :scss
output_style = :compressed
relative_assets = true

on_stylesheet_saved do |file|
  if File.exists?(file)
    filename = File.basename(file, File.extname(file))
    File.rename(file, "css" + "/" + filename + ".min" + File.extname(file))
  end
end