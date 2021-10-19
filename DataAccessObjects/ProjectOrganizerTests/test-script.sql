-- Delete all of the data
ALTER TABLE project_employee DROP CONSTRAINT FK__project_e__emplo__2E1BDC42
ALTER TABLE employee DROP CONSTRAINT fk_employee_department
ALTER TABLE project_employee DROP CONSTRAINT FK__project_e__proje__2D27B809;
DELETE FROM employee;
DELETE FROM project;
DELETE FROM department;

--SET IDENTITY_INSERT department ON -- Identity insert lets us specify the ID of an auto-generated PK
-- Insert fake department
INSERT INTO department (name)
VALUES ('Department of Doing the Code');

--SET IDENTITY_INSERT employee ON
-- Insert fake employee
INSERT INTO employee (employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date)
VALUES (1, 1, 'Caleb', 'Gaffney', 'Code Guy', '07/14/1992', '10/18/2021');

--SET IDENTITY_INSERT project ON
-- Insert a fake project
INSERT INTO project (name, from_date, to_date) 
VALUES ('Doing Code Stuff', '10/18/2021', '5/13/2026');

--Assign Employee to project
INSERT INTO project_employee (project_id, employee_id)
VALUES (1, 1);

--SET IDENTITY_INSERT department OFF

--SET IDENTITY_INSERT employee OFF
--SET IDENTITY_INSERT project OFF

