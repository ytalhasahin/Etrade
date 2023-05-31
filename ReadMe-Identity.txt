Identity Ýþlem Adýmlarý-M.AspNetCore.Identity kütüphanesiyle

1-Nuget Package'dan Identity Kütüphanesi eklenir.
   -Microsoft.AspNetCore.Identity
   -Microsoft.AspNetCore.Identity.EntityFramework

2-User ve Role Modelleri Eklenir

3-DbContext classýnýz IdentityDbContext<TUser,TRole,Tkey> classýndan kalýtým alýr.

4-Migration iþlemi yapýlýr
   Nuget Package Console da
       add-migration m
       update-database

5-Program.cs de IdentityCookie ayarý yapýlýr.

6-Core katmanýnda service classlarý kullanýlýr. Classlarýn metodlarý async dur.
   -UserManager<User>
   -RoleManager<Role>
   -SignManager<User>