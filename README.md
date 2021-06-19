# WeatherApp_cc

This program is a simple web interface that interacts with the https://openweathermap.org api. My reson for using this
specific source is that its simple, extremely well known, and provides detailed weather decriptions for 
over 200,000 cities (as per the site), and the free tier allows 60 calls per min, you can really delvelop a robust web app with this service.
As of the current build of this project, the api query string is built using the City and State inputs from
the Weather view and user input in the SignUp view.

# Instructions
To effectively use this interface, the Weather page needs a full (correct) 'City and State' name, the api will error
on strings less than 3 characters or null inputs. This also applies to the sign up page as well. 

## Functionality
- When initally signing up successfully, the user will be taken to the Weather view page, where a popup will show for 5 secs
and prompt the user to get the weather from their current location (use the location initially given in the signup page stored in DB). A row
is then generated in the table to showcase the current weather(converted to Fahrenheit) and weather description.

- For each new location entered, the current locations (if any) are updated. 

- When logging back into the site the, previously stored locations are updated (if any).
