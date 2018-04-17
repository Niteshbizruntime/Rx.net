package com.bizruntime;

import org.apache.ignite.Ignite;
import org.apache.ignite.IgniteCache;
import org.apache.ignite.IgniteException;
import org.apache.ignite.Ignition;
public class HelloWorld {
  public static void main(String[] args)  {
    try (Ignite ignite = Ignition.start()) {
      ignite.cluster().active(true);
      IgniteCache<Integer, String> cache = ignite.getOrCreateCache("myCache");
      cache.put(1, "Hello");
      cache.put(2, "World!");
      ignite.compute().broadcast(()->System.out.println(cache.get(1) + " " + cache.get(2)));
    }
    catch(IgniteException ex)
    {
    	System.out.print(ex.getMessage());
    }
  }
}
