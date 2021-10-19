-- Delete all of the data
DELETE FROM employee;
DELETE FROM project;
DELETE FROM department;

SET IDENTITY_INSERT department ON -- Identity insert lets us specify the ID of an auto-generated PK

-- Insert fake department
INSERT INTO department (department_id, name)
VALUES (1, 'Department of Doing the Code');

SET IDENTITY_INSERT department OFF

SET IDENTITY_INSERT employee ON
-- Insert fake employee
INSERT INTO employee (employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date)
VALUES (1, 1, 'Caleb', 'Gaffney', 'Code Guy', '07/14/1992', '10/18/2021');
SET IDENTITY_INSERT employee OFF

SET IDENTITY_INSERT project ON
-- Insert a fake project
INSERT INTO project (project_id, name, from_date, to_date) 
VALUES (1, 'Doing Code Stuff', '10/18/2021', '5/13/2026');
SET IDENTITY_INSERT project OFF

--Assign Employee to project
INSERT INTO project_employee (project_id, employee_id)
VALUES (1, 1);

