ALTER TABLE category_venue DROP CONSTRAINT FK__category___venue__34C8D9D1;
ALTER TABLE venue DROP CONSTRAINT FK__venue__city_id__31EC6D26;
ALTER TABLE space DROP CONSTRAINT FK__space__venue_id__32E0915F;
ALTER TABLE reservation DROP CONSTRAINT FK__reservati__space__36B12243;
DELETE FROM venue;
DELETE FROM space;
DELETE FROM reservation;
DELETE FROM category_venue;

INSERT INTO venue (name, city_id, description)
VALUES ('Test', 2, 'Test');

INSERT INTO space (venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy)
VALUES (1, 'OpenTest', 1, NULL, NULL, 1.00, 5),
	   (1, 'RestrictedTest', 0, 05, 09, 100.00, 100);

INSERT INTO reservation (space_id, number_of_attendees, start_date, end_date, reserved_for)
VALUES (1, 3, '02/14/2021', '02/18/2021', 'Test');
SET IDENTITY_INSERT reservation OFF

INSERT INTO category_venue (venue_id, category_id)
VALUES (1, 1),
	   (1, 6),
	   (2, 4);