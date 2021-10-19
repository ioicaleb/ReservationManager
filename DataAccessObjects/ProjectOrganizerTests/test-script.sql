-- Delete all of the data
DELETE FROM EmployeeDB;
UPDATE employee SET employee_id = NULL;
UPDATE project SET project_id = NULL;
UPDATE department SET department_id = NULL;
DELETE FROM employee;
DELETE FROM project;
DELETE FROM department;

SET IDENTITY_INSERT city ON -- Identity insert lets us specify the ID of an auto-generated PK

-- Insert fake department
INSERT INTO department (department_id, name)
VALUES (1, 'Department of Doing the Code');

-- Insert fake employee
INSERT INTO employee (employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date)
VALUES (1, 1, 'Caleb', 'Gaffney', 'Code Guy', '07/14/1992', '10/18/2021');

-- Insert a fake project
INSERT INTO project (project_id, name, from_date, to_date) 
VALUES (1, 'Doing Code Stuff', '10/18/2021', '5/13/2026');

--Assign Employee to project
INSERT INTO project_employee (project_id, employee_id)
VALUES (1, 1);

SET IDENTITY_INSERT city OFF
