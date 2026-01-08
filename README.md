#Hospital API 

** IT will manages - 
- users
-   create with password protected
-    login ( jwt token authentication)
- Patient
-   create patient using JWT Token for authorization , when user login , he will get the token for further uses
-   get patient
-   get patient by id

- Doctor
-     register doctor
-     get doctor

- Appointment
-   book an appoitment for a patient with a doctor doctor for specific time
-   get all appointment
-   get appointment details by date
-   get appointment details by doctor 
-   



HOW TO RUN

1- open the solution in visual studio 2022
2- change connection string which is in appsetting.json file  (Only to change the server name as of your sql server management studio)
3 - Run db migration 
    - add-migration "something"
    -update-database 
    (In your package console window)

4.click on run -  on the top of visualstudion  button
