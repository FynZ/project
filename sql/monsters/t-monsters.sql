-- auto-generated definition
create table t_monsters
(
    id        serial      not null
        constraint t_monsters_pk
            primary key,
    name      varchar(50) not null,
    slug      varchar(50) not null,
    min_level integer     not null,
    max_level integer     not null,
    ankama_id integer
);

alter table t_monsters
    owner to admin;

create unique index t_monsters_id_uindex
    on t_monsters (id);

