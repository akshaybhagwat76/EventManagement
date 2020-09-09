select * from sys.tables where name = 'Vendor'

select 'userbody.AppendLine(String.Format("' + name + ':{' + cast(column_id as varchar(3)) + '}", vendor.' + name + '));' 
from sys.columns where object_id =1765581328
order by column_id