-- Delete all of the data
ALTER TABLE project_employee DROP CONSTRAINT FK__project_e__emplo__2E1BDC42
ALTER TABLE employee DROP CONSTRAINT fk_employee_department
ALTER TABLE project_employee DROP CONSTRAINT FK__project_e__proje__2D27B809;
DELETE FROM employee;
DELETE FROM project;
DELETE FROM department;
DELETE FROM project_employee;


SET IDENTITY_INSERT department ON
-- Insert fake department
INSERT INTO department (department_id, name)
VALUES (1,'Department of Doing the Code');
SET IDENTITY_INSERT department OFF

SET IDENTITY_INSERT employee ON
-- Insert fake employee
INSERT INTO employee (employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date)
VALUES (1, 1, 'Caleb', 'Gaffney', 'Code Guy', '07/14/1992', '10/18/2021'),
	   (2, 1, 'Kim', 'Corson', 'Code Gal', '01/01/2000', '9/19/2021');
SET IDENTITY_INSERT employee OFF

SET IDENTITY_INSERT project ON
-- Insert a fake project
INSERT INTO project (project_id, name, from_date, to_date) 
VALUES (1, 'Doing Code Stuff', '10/18/2021', '5/13/2026');
SET IDENTITY_INSERT project OFF

