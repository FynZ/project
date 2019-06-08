-- auto-generated definition
create table t_user_roles
(
    user_id integer not null
        constraint t_user_roles_t_users_id_fk
            references t_users
            on update cascade on delete cascade,
    role    role    not null
);

alter table t_user_roles
    owner to admin;

