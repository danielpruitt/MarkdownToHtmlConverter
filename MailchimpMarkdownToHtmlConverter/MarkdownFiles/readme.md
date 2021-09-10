# Mailchimp Assessment

##Objective: Convert a Markdown file to Html 

In this console app a small set of Markdown syntax can be converted to HTML Elements. 

The app is made in .NET Core and uses no external libraries packages other than the standard System libraries. 
When building the app, I used different syntax and methods to show some diversity in the code but also for readability and knowledge of the system. 

Throughout the code, there are comments to explain any reasoning for the reason I wrote the methods the way I did. 

###### MVP
The base version of this app takes sample-one.md and sample-two.md and parses them into html. 
This readme can also be parsed. 

###### Feature Creep 
- be able to specify a location on the machine to pick up new md files
- for larger files, parallel threads in order to make this more efficient 
- handle the different styles of lists and other elements 
- save the generated html to a new file


