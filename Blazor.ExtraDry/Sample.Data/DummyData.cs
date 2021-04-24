using Sample.Shared;
using System;
using System.Threading.Tasks;

namespace Sample.Data
{
    public class DummyData
    {


        public async Task PopulateEmployees(SampleContext database, int count)
        {
            for(int i = 0; i < count; ++i) {
                var first = PickRandom(firstNames);
                var last = PickRandom(lastNames);
                var employee = new Employee { FirstName = first, LastName = last };
                database.Employees.Add(employee);
            }
            await database.SaveChangesAsync();
        }

        private string PickRandom(string[] candidates) => candidates[random.Next(0, candidates.Length)];

        private readonly Random random = new(123);

        private readonly string[] firstNames = {"James", "John", "Robert", "Michael", "William", "David", "Richard",
            "Joseph", "Thomas", "Charles",  "Christopher", "Daniel", "Matthew", "Anthony", "Donald", "Mark",
            "Paul",  "Steven", "Andrew", "Kenneth", "Joshua", "Kevin", "Brian", "George", "Edward", "Ronald",
            "Timothy", "Jason", "Jeffrey", "Ryan", "Jacob", "Gary", "Nicholas", "Eric", "Jonathan", "Stephen",
            "Larry", "Justin", "Scott", "Brandon", "Benjamin", "Samuel", "Frank", "Gregory", "Raymond",
            "Alexander", "Patrick", "Jack", "Dennis", "Jerry", "Mary", "Patricia", "Jennifer", "Linda",
            "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen", "Nancy", "Lisa", "Margaret", "Betty",
            "Sandra", "Ashley", "Dorothy", "Kimberly", "Emily", "Donna", "Michelle", "Carol", "Amanda", 
            "Melissa", "Deborah", "Stephanie", "Rebecca", "Laura", "Sharon", "Cynthia", "Kathleen", "Amy",
            "Shirley", "Angela", "Helen", "Anna", "Brenda", "Pamela", "Nicole", "Samantha", "Katherine", 
            "Emma", "Ruth", "Christine", "Catherine", "Debra", "Rachel", "Carolyn", "Janet", "Virginia" };


        private readonly string[] lastNames = {"Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis",
            "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson", "Taylor", "Thomas", "Hernandez", "Moore",
            "Martin", "Jackson", "Thompson", "White", "Lopez", "Lee", "Gonzalez", "Harris", "Clark", "Lewis",
            "Robinson", "Walker", "Perez", "Hall", "Young", "Allen", "Sanchez", "Wright", "King", "Scott",
            "Green", "Baker", "Adams", "Nelson", "Hill", "Ramirez", "Campbell", "Mitchell", "Roberts", "Carter",
            "Phillips", "Evans", "Turner", "Torres", "Parker", "Collins", "Edwards", "Stewart", "Flores",
            "Morris", "Nguyen", "Murphy", "Rivera", "Cook", "Rogers", "Morgan", "Peterson", "Cooper", "Reed",
            "Bailey", "Bell", "Gomez", "Kelly", "Howard", "Ward", "Cox", "Diaz", "Richardson", "Wood", "Watson",
            "Brooks", "Bennett", "Gray", "James", "Reyes", "Cruz", "Hughes", "Price", "Myers", "Long", "Foster",
            "Sanders", "Ross", "Morales", "Powell", "Sullivan", "Russell", "Ortiz", "Jenkins", "Gutierrez",
            "Perry", "Butler", "Barnes", "Fisher" };


    }
}
