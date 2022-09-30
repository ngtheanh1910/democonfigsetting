use egov
go

Create procedure [dbo].[AddSetting]  
(  
   @Url nvarchar (255),  
   @Token varchar (255),  
   @NameUnit nvarchar (255)  
)  
as  
begin  
   Insert into Setting values(@Url,@Token,@NameUnit)  
End 

Create Procedure [dbo].[GetSetting]  
as  
begin  
   select *from Setting  
End 

Create procedure [dbo].[UpdateSetting]  
(  
   @SettingId int,  
   @Url nvarchar (255),  
   @Token varchar (255),  
   @NameUnit nvarchar (255)   
)  
as  
begin  
   Update Setting   
   set Url=@Url,  
   Token=@Token,  
   NameUnit=@NameUnit  
   where Id=@SettingId  
End 

Create procedure [dbo].[DeleteSetting]  
(  
   @SettingId int 
)  
as   
begin  
   Delete from Setting where Id=@SettingId   
End 