CREATE DATABASE departments;

CREATE TABLE public.department (
	department_id serial4 NOT NULL,
	name varchar NOT NULL,
	status int4 NOT NULL DEFAULT 1,
	parent_id int4 NULL,
	CONSTRAINT department_pkey PRIMARY KEY (department_id),
	CONSTRAINT department_fk FOREIGN KEY (parent_id) REFERENCES public.department(department_id) ON DELETE SET NULL
);