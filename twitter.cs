using System;

class Program
{
    static void Main(string[] args)
    {

        int MAX_USERS = 100;
        int MAX_POSTS = MAX_USERS * 100;
        int MAX_FOLLOWS = MAX_USERS * (MAX_USERS + 1) / 2;

        string[] users = new string[MAX_USERS];
        int userCount = 0;

        string[] posts = new string[MAX_POSTS];
        string[] postAuthors = new string[MAX_POSTS];
        int postCount = 0;

        string[] followers = new string[MAX_FOLLOWS];
        string[] followees = new string[MAX_FOLLOWS];
        int followCount = 0;


        AddUser("wormik", users, ref userCount);
        AddUser("pepa", users, ref userCount);
        AddUser("jana", users, ref userCount);
        Console.WriteLine($"Pocet uzivatelu: {userCount}");


        AddPost("Ahoj svete!", "wormik", posts, postAuthors, ref postCount);
        AddPost("Dnes je hezky.", "pepa", posts, postAuthors, ref postCount);
        AddPost("Mam rada kavu.", "jana", posts, postAuthors, ref postCount);
        AddPost("Programovani je super.", "wormik", posts, postAuthors, ref postCount);

        Console.WriteLine("\n--- Prispevky uzivatele wormik ---");
        PrintArray(GetUserPosts("wormik", posts, postAuthors, postCount));


        Console.WriteLine("\n--- Pridavani sledovani ---");
        AddFollow("pepa", "wormik", followers, followees, ref followCount);
        AddFollow("pepa", "jana", followers, followees, ref followCount);
        AddFollow("jana", "wormik", followers, followees, ref followCount);
        AddFollow("pepa", "pepa", followers, followees, ref followCount);
        AddFollow("pepa", "wormik", followers, followees, ref followCount);

        Console.WriteLine($"\nKoho sleduje Pepa:");
        PrintArray(GetUserFollows("pepa", followers, followees, followCount));

        Console.WriteLine($"\nKdo sleduje Wormika:");
        PrintArray(GetUserFollowers("wormik", followers, followees, followCount));


        Console.WriteLine("\n--- Timeline uzivatele Pepa (co napsali ti, ktere sleduje) ---");
        PrintArray(GetUserTimeline("pepa", posts, postAuthors, postCount, followers, followees, followCount));


        Console.WriteLine("\n--- Odebrani sledovani (Pepa prestane sledovat Janu) ---");
        RemoveFollow("pepa", "jana", followers, followees, ref followCount);

        Console.WriteLine("Koho sleduje Pepa po odebrani:");
        PrintArray(GetUserFollows("pepa", followers, followees, followCount));

        Console.ReadLine();
    }

    static void PrintArray(string[] data)
    {
        if (data.Length == 0) Console.WriteLine("(zadna data)");
        foreach (var item in data)
        {
            Console.WriteLine($"- {item}");
        }
    }


    static bool AddValue(string value, string[] data, int count)
    {
        if (count >= data.Length)
        {
            Console.WriteLine("I'm afraid I can't do that.");
            return false;
        }

        data[count] = value;
        return true;
    }

    static bool RemoveValue(string[] data, int index, int count)
    {
        if (index < 0 || index >= count)
        {
            Console.WriteLine("I'm afraid I can't do that.");
            return false;
        }

        for (int i = index; i < count - 1; i += 1)
        {
            data[i] = data[i + 1];
        }
        data[count - 1] = "";
        return true;
    }

    static void AddUser(string username, string[] users, ref int userCount)
    {
        int index = Array.IndexOf(users, username);
        if (index >= 0)
        {
            Console.WriteLine($"User {username} already exists.");
            return;
        }

        if (AddValue(username, users, userCount))
        {
            userCount += 1;
        }
    }

    static void RemoveUser(string username, string[] users, ref int userCount)
    {
        int index = Array.IndexOf(users, username);
        if (index < 0)
        {
            Console.WriteLine("User does not exist.");
            return;
        }

        if (index >= 0 && RemoveValue(users, index, userCount))
        {
            userCount -= 1;
        }
    }


    static void AddPost(string post, string author, string[] posts, string[] postAuthors, ref int postCount)
    {

        if (postCount >= posts.Length)
        {
            Console.WriteLine("Post storage is full.");
            return;
        }


        posts[postCount] = post;
        postAuthors[postCount] = author;
        postCount++;
    }

    static string[] GetUserPosts(string user, string[] posts, string[] postAuthors, int postCount)
    {

        int count = 0;
        for (int i = 0; i < postCount; i++)
        {
            if (postAuthors[i] == user)
            {
                count++;
            }
        }


        string[] result = new string[count];
        int resultIndex = 0;
        for (int i = 0; i < postCount; i++)
        {
            if (postAuthors[i] == user)
            {
                result[resultIndex] = posts[i];
                resultIndex++;
            }
        }
        return result;
    }

    static void AddFollow(string follower, string followee, string[] followers, string[] followees, ref int followCount)
    {

        if (follower == followee)
        {
            Console.WriteLine("User cannot follow themselves.");
            return;
        }


        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == follower && followees[i] == followee)
            {
                Console.WriteLine("Already following.");
                return;
            }
        }


        if (followCount >= followers.Length)
        {
            Console.WriteLine("Follow storage is full.");
            return;
        }


        followers[followCount] = follower;
        followees[followCount] = followee;
        followCount++;
    }

    static void RemoveFollow(string follower, string followee, string[] followers, string[] followees, ref int followCount)
    {
        int index = -1;

        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == follower && followees[i] == followee)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Console.WriteLine("Follow relationship does not exist.");
            return;
        }


        for (int i = index; i < followCount - 1; i++)
        {
            followers[i] = followers[i + 1];
            followees[i] = followees[i + 1];
        }

        followers[followCount - 1] = "";
        followees[followCount - 1] = "";

        followCount--;
    }

    static string[] GetUserFollows(string user, string[] followers, string[] followees, int followCount)
    {

        int count = 0;
        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == user) count++;
        }

        string[] result = new string[count];
        int resultIndex = 0;
        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == user)
            {
                result[resultIndex] = followees[i];
                resultIndex++;
            }
        }
        return result;
    }

    static string[] GetUserFollowers(string user, string[] followers, string[] followees, int followCount)
    {

        int count = 0;
        for (int i = 0; i < followCount; i++)
        {
            if (followees[i] == user) count++;
        }

        string[] result = new string[count];
        int resultIndex = 0;
        for (int i = 0; i < followCount; i++)
        {
            if (followees[i] == user)
            {
                result[resultIndex] = followers[i];
                resultIndex++;
            }
        }
        return result;
    }


    static string[] GetUserTimeline(string user, string[] posts, string[] postAuthors, int postCount, string[] followers, string[] followees, int followCount)
    {

        string[] followedUsers = GetUserFollows(user, followers, followees, followCount);


        int timelineCount = 0;
        for (int i = 0; i < postCount; i++)
        {

            if (Array.IndexOf(followedUsers, postAuthors[i]) >= 0)
            {
                timelineCount++;
            }
        }


        string[] timeline = new string[timelineCount];
        int index = 0;
        for (int i = 0; i < postCount; i++)
        {
            if (Array.IndexOf(followedUsers, postAuthors[i]) >= 0)
            {

                timeline[index] = posts[i];
                index++;
            }
        }

        return timeline;
    }
}

