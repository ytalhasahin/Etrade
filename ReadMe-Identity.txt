Identity ��lem Ad�mlar�-M.AspNetCore.Identity k�t�phanesiyle

1-Nuget Package'dan Identity K�t�phanesi eklenir.
   -Microsoft.AspNetCore.Identity
   -Microsoft.AspNetCore.Identity.EntityFramework

2-User ve Role Modelleri Eklenir

3-DbContext class�n�z IdentityDbContext<TUser,TRole,Tkey> class�ndan kal�t�m al�r.

4-Migration i�lemi yap�l�r
   Nuget Package Console da
       add-migration m
       update-database

5-Program.cs de IdentityCookie ayar� yap�l�r.

6-Core katman�nda service classlar� kullan�l�r. Classlar�n metodlar� async dur.
   -UserManager<User>
   -RoleManager<Role>
   -SignManager<User>