package demo;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class Hello 
{

	static Connection conn = null;
	
	public void Getconnection() {
		
		try {
			Class.forName("org.apache.ignite.IgniteJdbcThinDriver");
			
		} catch (ClassNotFoundException e) {
			
			e.printStackTrace();
		}

		
		try {
			conn = DriverManager.getConnection(
			    "jdbc:ignite:thin://192.168.1.6/");
			 
		} catch (SQLException e) {
			
			e.printStackTrace();
		}
	}
	
	
	public void CreateTable() {
		
		
		
		try (Statement stmt = conn.createStatement()) {

//		 
//		    stmt.executeUpdate("CREATE TABLE City (id LONG PRIMARY KEY, name VARCHAR) ");// +
//		 //   " WITH \"template=replicated\"");
//
//		    
//		    stmt.executeUpdate("CREATE TABLE Person (" +
//		    " id LONG, name VARCHAR, city_id LONG, " +
//		    " PRIMARY KEY (id, city_id)) ");// +
//		    //" WITH \"backups=1, affinityKey=city_id\"");
		     
			stmt.executeUpdate("CREATE TABLE City (" +  
					" id LONG PRIMARY KEY, name VARCHAR) " + 
					" WITH \"template=replicated\""); 
					stmt.executeUpdate("CREATE TABLE Person (" + 
			 		" id LONG, name VARCHAR, city_id LONG, " + 
			 		" PRIMARY KEY (id, city_id)) " + 
			 		" WITH \"backups=1, affinityKey=city_id\""); 
			 		stmt.executeUpdate("CREATE INDEX idx_city_name ON City (name)"); 
			 		stmt.executeUpdate("CREATE INDEX idx_person_name ON Person (name)"); 
		   
			System.out.println("Hello");
		} catch (SQLException e) {
		
			e.printStackTrace();
			 System.out.println(e.getMessage());
		}
	}
	
	
	public void CreateIndexs() {
		
		try (Statement stmt = conn.createStatement()) {

		    
		    stmt.executeUpdate("CREATE INDEX idx_city_name ON City (name)");

		   
		    stmt.executeUpdate("CREATE INDEX idx_person_name ON Person (name)");
		} catch (SQLException e) {
			
			e.printStackTrace();
		}
	}
	
	
	public void InsertData() {
		
		try (PreparedStatement stmt =
		conn.prepareStatement("INSERT INTO City (id, name) VALUES (?, ?)")) {

		    stmt.setLong(1, 1L);
		    stmt.setString(2, "Forest Hill");
		    stmt.executeUpdate();

		    stmt.setLong(1, 2L);
		    stmt.setString(2, "Denver");
		    stmt.executeUpdate();

		    stmt.setLong(1, 3L);
		    stmt.setString(2, "St. Petersburg");
		    stmt.executeUpdate();
		} catch (SQLException e) {
		
			e.printStackTrace();
		}

		
		try (PreparedStatement stmt =
		conn.prepareStatement("INSERT INTO Person (id, name, city_id) VALUES (?, ?, ?)")) {

		    stmt.setLong(1, 1L);
		    stmt.setString(2, "John Doe");
		    stmt.setLong(3, 3L);
		    stmt.executeUpdate();

		    stmt.setLong(1, 2L);
		    stmt.setString(2, "Jane Roe");
		    stmt.setLong(3, 2L);
		    stmt.executeUpdate();

		    stmt.setLong(1, 3L);
		    stmt.setString(2, "Mary Major");
		    stmt.setLong(3, 1L);
		    stmt.executeUpdate();

		    stmt.setLong(1, 4L);
		    stmt.setString(2, "Richard Miles");
		    stmt.setLong(3, 2L);
		    stmt.executeUpdate();
		} catch (SQLException e) {
			
			e.printStackTrace();
		}
	}
	
	
	public void ShowData() {
		
		try (Statement stmt = conn.createStatement()) {
		    try (ResultSet rs =
		    stmt.executeQuery("SELECT p.name, c.name " +
		    " FROM Person p, City c " +
		    " WHERE p.city_id = c.id")) {

		      System.out.println("Query result:");

		      while (rs.next())
		         System.out.println(">>>    " + rs.getString(1) +
		            ", " + rs.getString(2));
		    }
		} catch (SQLException e) {
			
			e.printStackTrace();
		}
	}
	
	
	public void UpdateData() {
		
		try (Statement stmt = conn.createStatement()) {

		    
		    stmt.executeUpdate("UPDATE City SET name = 'Foster City' WHERE id = 2");
		} catch (SQLException e) {
			
			e.printStackTrace();
		}
	}
	
	
	
	public void RemoveData() {
	
		try (Statement stmt = conn.createStatement()) {

		   
		    stmt.executeUpdate("DELETE FROM Person WHERE name = 'John Doe'");
		} catch (SQLException e) {
			
			e.printStackTrace();
		}
	}
	
	
	
	
	
	
	
	
	
	
	public static void main(String[] args) {
		
		
		Hello object = new Hello();
		object.Getconnection();
		
        object.CreateTable();
        
		//object.CreateIndexs();
      	object.InsertData();
       	object.ShowData();
		//object.UpdateData();
		//object.RemoveData();
        if(conn!=null)
        {
        	try {
				conn.close();
			} catch (SQLException e) {
				
				e.printStackTrace();
			}
        }

	}

}