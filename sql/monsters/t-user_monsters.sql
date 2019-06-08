-- auto-generated definition
create table t_user_monsters
(
    id         serial                not null
        constraint t_user_monsters_pk
            primary key,
    monster_id integer               not null,
    count      integer default 0     not null,
    search     boolean default true  not null,
    propose    boolean default false not null,
    user_id    integer               not null
);

alter table t_user_monsters
    owner to admin;

create unique index t_user_monsters_id_uindex
    on t_user_monsters (id);

