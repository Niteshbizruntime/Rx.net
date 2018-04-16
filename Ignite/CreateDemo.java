package demo;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.sql.Statement;

public class CreateDemo 
{
	public static void main(String[] args) throws ClassNotFoundException,SQLException  
	 	{ 
			Class.forName("org.apache.ignite.IgniteJdbcThinDriver"); 
			Connection conn = DriverManager.getConnection("jdbc:ignite:thin://192.168.1.5/"); 
 		try (Statement stmt = conn.createStatement())
 		{ 
	 		stmt.executeUpdate("CREATE TABLE Person1 (" +  
			" id int PRIMARY KEY, name VARCHAR) " + " WITH \"template=replicated\""); 
	 		stmt.executeUpdate("CREATE INDEX person ON Person1 (name)"); 
	 		} 
	 	System.out.println("Tables created"); 
	 	} 

}
