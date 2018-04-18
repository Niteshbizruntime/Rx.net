

package org.apache.ignite.examples.computegrid;

import java.util.Collection;
import org.apache.ignite.Ignite;
import org.apache.ignite.IgniteException;
import org.apache.ignite.Ignition;
import org.apache.ignite.examples.ExampleNodeStartup;


public class ComputeBroadcastExample {
   
    public static void main(String[] args) throws IgniteException {
        try (Ignite ignite = Ignition.start("examples/config/example-ignite.xml")) {
            System.out.println();
            System.out.println(">>> Compute broadcast example started.");

            // Print hello message on all nodes.
            hello(ignite);

            // Gather system info from all nodes.
            gatherSystemInfo(ignite);
       }
    }

    
    private static void hello(Ignite ignite) throws IgniteException {
        // Print out hello message on all nodes.
        ignite.compute().broadcast(() -> {
            System.out.println();
            System.out.println(">>> Hello Node! :)");
        });

        System.out.println();
        System.out.println(">>> Check all nodes for hello message output.");
    }

   
    private static void gatherSystemInfo(Ignite ignite) throws IgniteException {
        // Gather system info from all nodes.
        Collection<String> res = ignite.compute().broadcast(() -> {
            System.out.println();
            System.out.println("Executing task on node: " + ignite.cluster().localNode().id());

            return "Node ID: " + ignite.cluster().localNode().id() + "\n" +
                "OS: " + System.getProperty("os.name") + " " + System.getProperty("os.version") + " " +
                System.getProperty("os.arch") + "\n" +
                "User: " + System.getProperty("user.name") + "\n" +
                "JRE: " + System.getProperty("java.runtime.name") + " " +
                System.getProperty("java.runtime.version");
        });

        // Print result.
        System.out.println();
        System.out.println("Nodes system information:");
        System.out.println();

        res.forEach(r -> {
            System.out.println(r);
            System.out.println();
        });
    }
}