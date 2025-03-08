--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4 (Debian 17.4-1.pgdg120+2)
-- Dumped by pg_dump version 17.4 (Debian 17.4-1.pgdg120+2)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: book_type; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.book_type AS ENUM (
    'buy',
    'rent',
    'free'
);


ALTER TYPE public.book_type OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: addresses; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.addresses (
    id integer NOT NULL,
    city_id integer,
    street character varying(150) NOT NULL
);


ALTER TABLE public.addresses OWNER TO postgres;

--
-- Name: addresses_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.addresses_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.addresses_id_seq OWNER TO postgres;

--
-- Name: addresses_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.addresses_id_seq OWNED BY public.addresses.id;


--
-- Name: authors; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.authors (
    id integer NOT NULL,
    name character varying(150) NOT NULL,
    birthdate date NOT NULL
);


ALTER TABLE public.authors OWNER TO postgres;

--
-- Name: authors_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.authors_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.authors_id_seq OWNER TO postgres;

--
-- Name: authors_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.authors_id_seq OWNED BY public.authors.id;


--
-- Name: books; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.books (
    id integer NOT NULL,
    library_id integer,
    name character varying(200) NOT NULL,
    author_id integer
);


ALTER TABLE public.books OWNER TO postgres;

--
-- Name: books_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.books_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.books_id_seq OWNER TO postgres;

--
-- Name: books_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.books_id_seq OWNED BY public.books.id;


--
-- Name: cities; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.cities (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    zip_code character varying(20) NOT NULL
);


ALTER TABLE public.cities OWNER TO postgres;

--
-- Name: cities_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.cities_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.cities_id_seq OWNER TO postgres;

--
-- Name: cities_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.cities_id_seq OWNED BY public.cities.id;


--
-- Name: libraries; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.libraries (
    id integer NOT NULL,
    name character varying(150) NOT NULL,
    type public.book_type NOT NULL
);


ALTER TABLE public.libraries OWNER TO postgres;

--
-- Name: libraries_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.libraries_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.libraries_id_seq OWNER TO postgres;

--
-- Name: libraries_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.libraries_id_seq OWNED BY public.libraries.id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    username character varying(50) NOT NULL,
    email character varying(100) NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_id_seq OWNER TO postgres;

--
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- Name: addresses id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.addresses ALTER COLUMN id SET DEFAULT nextval('public.addresses_id_seq'::regclass);


--
-- Name: authors id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.authors ALTER COLUMN id SET DEFAULT nextval('public.authors_id_seq'::regclass);


--
-- Name: books id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books ALTER COLUMN id SET DEFAULT nextval('public.books_id_seq'::regclass);


--
-- Name: cities id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cities ALTER COLUMN id SET DEFAULT nextval('public.cities_id_seq'::regclass);


--
-- Name: libraries id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.libraries ALTER COLUMN id SET DEFAULT nextval('public.libraries_id_seq'::regclass);


--
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- Data for Name: addresses; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.addresses (id, city_id, street) FROM stdin;
1	1	123 Main St
2	1	456 Broadway
3	1	789 Park Ave
4	2	101 Hollywood Blvd
5	2	202 Sunset Blvd
6	2	303 Melrose Ave
7	3	111 Michigan Ave
8	3	222 State St
9	3	333 Wacker Dr
10	4	444 Westheimer Rd
11	4	555 Richmond Ave
12	4	666 Kirby Dr
13	5	777 Market St
14	5	888 Mission St
15	5	999 Van Ness Ave
16	1	135 5th Ave
17	2	246 Rodeo Dr
18	3	357 Lake Shore Dr
19	4	468 Memorial Dr
20	5	579 Golden Gate Ave
\.


--
-- Data for Name: authors; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.authors (id, name, birthdate) FROM stdin;
1	George Orwell	1903-06-25
2	Jane Austen	1775-12-16
3	Mark Twain	1835-11-30
4	Virginia Woolf	1882-01-25
5	Ernest Hemingway	1899-07-21
6	F. Scott Fitzgerald	1896-09-24
7	J.K. Rowling	1965-07-31
8	Stephen King	1947-09-21
9	Agatha Christie	1890-09-15
10	Leo Tolstoy	1828-09-09
\.


--
-- Data for Name: books; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.books (id, library_id, name, author_id) FROM stdin;
1	1	1984	1
2	2	Pride and Prejudice	2
3	3	Adventures of Huckleberry Finn	3
4	4	Mrs Dalloway	4
5	5	The Old Man and the Sea	5
6	1	The Great Gatsby	6
7	2	Harry Potter and the Sorcerer's Stone	7
8	3	The Shining	8
9	4	Murder on the Orient Express	9
10	5	War and Peace	10
11	1	Animal Farm	1
12	2	Sense and Sensibility	2
13	3	The Adventures of Tom Sawyer	3
14	4	To the Lighthouse	4
15	5	A Farewell to Arms	5
16	1	Tender Is the Night	6
17	2	Harry Potter and the Chamber of Secrets	7
18	3	It	8
19	4	And Then There Were None	9
20	5	Anna Karenina	10
21	1	Coming Up for Air	1
22	2	Emma	2
23	3	Pudd'nhead Wilson	3
24	4	Orlando	4
25	5	For Whom the Bell Tolls	5
26	1	This Side of Paradise	6
27	2	Harry Potter and the Prisoner of Azkaban	7
28	3	Carrie	8
29	4	The ABC Murders	9
30	5	Resurrection	10
31	1	Keep the Aspidistra Flying	1
32	2	Northanger Abbey	2
33	3	The Prince and the Pauper	3
34	4	The Waves	4
35	5	The Sun Also Rises	5
36	1	The Beautiful and Damned	6
37	2	Harry Potter and the Goblet of Fire	7
38	3	Misery	8
39	4	Death on the Nile	9
40	5	Childhood	10
41	1	Burmese Days	1
42	2	Mansfield Park	2
43	3	Roughing It	3
44	4	Jacob's Room	4
45	5	Islands in the Stream	5
\.


--
-- Data for Name: cities; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.cities (id, name, zip_code) FROM stdin;
1	New York	10001
2	Los Angeles	90001
3	Chicago	60601
4	Houston	77001
5	San Francisco	94101
\.


--
-- Data for Name: libraries; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.libraries (id, name, type) FROM stdin;
1	Central Library	free
2	Downtown Bookstore	buy
3	University Library	free
4	Community Book Rental	rent
5	City Bookshop	buy
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, username, email, created_at) FROM stdin;
1	admin	admin@gmail.com	2025-03-06 22:32:17.707126
2	johndoe	john.doe@gmail.com	2025-03-07 18:34:26.961111
3	annasmith	anna.smith@yahoo.com	2025-03-07 18:34:26.961111
4	mike_j	mike.johnson@outlook.com	2025-03-07 18:34:26.961111
5	lisa_w	lisa.white@gmail.com	2025-03-07 18:34:26.961111
6	tommy89	tommy89@hotmail.com	2025-03-07 18:34:26.961111
7	sara.k	sara.karson@icloud.com	2025-03-07 18:34:26.961111
8	chris_p	chris.petersen@gmail.com	2025-03-07 18:34:26.961111
9	natalie_b	natalie.brown@yahoo.com	2025-03-07 18:34:26.961111
10	dave_m	dave.miller@company.com	2025-03-07 18:34:26.961111
11	emily_r	emily.rogers@gmail.com	2025-03-07 18:34:26.961111
12	kevin_c	kevin.carter@outlook.com	2025-03-07 18:34:26.961111
13	laura_h	laura.hill@yahoo.com	2025-03-07 18:34:26.961111
14	peter_g	peter.green@company.com	2025-03-07 18:34:26.961111
15	olivia_j	olivia.james@gmail.com	2025-03-07 18:34:26.961111
16	brian_s	brian.scott@outlook.com	2025-03-07 18:34:26.961111
17	rachel_t	rachel.turner@yahoo.com	2025-03-07 18:34:26.961111
18	steve_b	steve.baker@gmail.com	2025-03-07 18:34:26.961111
19	amanda_c	amanda.coleman@icloud.com	2025-03-07 18:34:26.961111
20	jason_l	jason.lee@company.com	2025-03-07 18:34:26.961111
21	megan_w	megan.white@yahoo.com	2025-03-07 18:34:26.961111
\.


--
-- Name: addresses_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.addresses_id_seq', 20, true);


--
-- Name: authors_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.authors_id_seq', 10, true);


--
-- Name: books_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.books_id_seq', 45, true);


--
-- Name: cities_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.cities_id_seq', 5, true);


--
-- Name: libraries_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.libraries_id_seq', 5, true);


--
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 21, true);


--
-- Name: addresses addresses_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.addresses
    ADD CONSTRAINT addresses_pkey PRIMARY KEY (id);


--
-- Name: authors authors_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.authors
    ADD CONSTRAINT authors_pkey PRIMARY KEY (id);


--
-- Name: books books_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books
    ADD CONSTRAINT books_pkey PRIMARY KEY (id);


--
-- Name: cities cities_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cities
    ADD CONSTRAINT cities_pkey PRIMARY KEY (id);


--
-- Name: libraries libraries_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.libraries
    ADD CONSTRAINT libraries_pkey PRIMARY KEY (id);


--
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- Name: users users_username_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_username_key UNIQUE (username);


--
-- Name: addresses addresses_city_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.addresses
    ADD CONSTRAINT addresses_city_id_fkey FOREIGN KEY (city_id) REFERENCES public.cities(id) ON DELETE CASCADE;


--
-- Name: books books_author_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books
    ADD CONSTRAINT books_author_id_fkey FOREIGN KEY (author_id) REFERENCES public.authors(id) ON DELETE CASCADE;


--
-- Name: books books_library_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books
    ADD CONSTRAINT books_library_id_fkey FOREIGN KEY (library_id) REFERENCES public.libraries(id) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

