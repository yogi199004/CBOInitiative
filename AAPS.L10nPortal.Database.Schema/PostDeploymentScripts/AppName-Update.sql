--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App27' where Name='Levvia1-v2.0.5'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App28' where Name='Levvia2-v2.0.5'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App29' where Name='Levvia3-v2.0.5'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App30' where Name='Levvia1-v2.1'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App31' where Name='Levvia2-v2.1'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App32' where Name='Levvia3-v2.1'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App47' where Name='r2Levvia1-v2.2'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App48' where Name='r2Levvia2-v2.2'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App49' where Name='r2Levvia3-v2.2'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App50' where Name='r2Levvia1-v3.0'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App51' where Name='r2Levvia2-v3.0'
--END
--Go

--IF '$(Environment)' NOT IN ('QA')
--Begin
--Update Application 
--set Name='App52' where Name='r2Levvia3-v3.0'
--END
--Go

--IF '$(Environment)'  IN ('STAGE')
--Begin
--Update Application 
--set Name='IntelaFE1' where Id=22
--END
--Go

--IF '$(Environment)'  IN ('STAGE')
--Begin
--Update Application 
--set Name='IntelaFE2' where Id=27
--END
--Go

--IF '$(Environment)'  IN ('STAGE')
--Begin
--Update Application 
--set Name='IntelaBE' where Id=28
--END
--Go

