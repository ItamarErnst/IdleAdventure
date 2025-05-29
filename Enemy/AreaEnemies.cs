namespace IdleAdventure.Enemy;

public class AreaEnemies
{
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)>
        caves = new()
        {
            // Cave
            (
                "Bat Swarm",
                new() { "swoops and scratches!", "emits a disorienting screech!" },
                "A swarm of bats erupts from the ceiling!",
                "The bats scatter into the dark.",
                "The swarm engulfs you entirely."
            ),
            (
                "Stone Golem",
                new() { "slams its fists!", "hurls a boulder!" },
                "A stone golem awakens with a rumble!",
                "It crumbles into rubble.",
                "You are crushed under its might."
            ),
            (
                "Cave Spider",
                new() { "lunges with venomous fangs!", "sprays sticky webbing!" },
                "A giant spider descends from above!",
                "It curls up and dies.",
                "You're cocooned in its web."
            ),
            (
                "Blind Mole Beast",
                new() { "burrows and strikes from below!", "lets out a piercing squeal!" },
                "The ground trembles—something is coming!",
                "It retreats into the earth.",
                "It drags you beneath the soil."
            ),
            (
                "Rock Serpent",
                new() { "lashes out with rocky scales!", "snaps its jaw with a crack!" },
                "A serpent slithers out of a fissure!",
                "It slinks away defeated.",
                "Its tail is the last thing you see."
            ),
        };
    
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)>
        desert = new()
        {
            // Desert
            (
                "Sand Wraith",
                new() { "slashes with claws of wind!", "vanishes and reappears!" },
                "A sandstorm reveals a wraith within!",
                "It dissipates like dust.",
                "The wind howls your final dirge."
            ),
            (
                "Scorpion King",
                new() { "stabs with its tail!", "clicks its claws with fury!" },
                "The dunes shift—a massive scorpion emerges!",
                "Its shell cracks as it falls.",
                "Poison floods your veins."
            ),
            (
                "Dune Bandit",
                new() { "fires an arrow!", "throws a sand bomb!" },
                "A bandit ambushes you from a rocky ledge!",
                "He retreats, wounded.",
                "He loots your remains."
            ),
            (
                "Mirage Djinn",
                new() { "laughs and casts illusions!", "engulfs you in flames!" },
                "A figure shimmers into view—it’s a djinn!",
                "It vanishes with a sigh.",
                "Your senses fade in fire."
            ),
            (
                "Cactus Beast",
                new() { "launches thorny spikes!", "rams with its bulk!" },
                "A cactus unroots itself—alive!",
                "It collapses into pulp.",
                "You're pierced from all sides."
            ),
        };
    
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> 
        forest = new()
    {
        // Forest
        (
            "Treant",
            new() { "swings a massive branch!", "calls roots from below!" },
            "The forest groans—a tree moves!",
            "It splinters and crashes down.",
            "Its roots claim your body."
        ),
        (
            "Feral Wolf",
            new() { "lunges with a growl!", "circles and bites!" },
            "A wolf howls—you're not alone.",
            "It limps away defeated.",
            "The pack feasts on you."
        ),
        (
            "Pixie Trickster",
            new() { "throws blinding dust!", "mimics your voice!" },
            "Laughter echoes among the trees.",
            "It vanishes into petals.",
            "You fall under a cruel spell."
        ),
        (
            "Forest Shade",
            new() { "emerges from a shadow!", "extends shadowy claws!" },
            "Darkness gathers—it forms a being!",
            "It melts into the trees.",
            "It whispers as you fall."
        ),
        (
            "Horned Stag",
            new() { "charges with its antlers!", "kicks with force!" },
            "A majestic but angry stag blocks your path!",
            "It retreats into the mist.",
            "You are trampled under hoof."
        )
    };
    
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> 
        ruins = new()
        {
            // Floating Ruins
            (
                "Sky Warden",
                new() { "unleashes a bolt of lightning!", "strikes with a floating spear!" },
                "A glowing sentinel descends from above!",
                "It fizzles out into static.",
                "You are cast from the skies."
            ),
            (
                "Aether Drake",
                new() { "breathes searing wind!", "lashes its tail midair!" },
                "A shimmering drake coils through the clouds!",
                "It lets out one final screech and vanishes.",
                "You plummet as the wind turns against you."
            ),
            (
                "Ancient Construct",
                new() { "fires arcane pulses!", "emits a wave of force!" },
                "A levitating relic activates with a hum!",
                "Its pieces scatter across the platform.",
                "Its energy implodes—taking you with it."
            ),
            (
                "Orb Weaver",
                new() { "conjures binding energy!", "launches radiant threads!" },
                "Light distorts—a creature of silk appears!",
                "Its web frays and dissolves.",
                "You are bound in stasis forever."
            ),
            (
                "Wind Phantom",
                new() { "slices through with gales!", "lets out a haunting wail!" },
                "A translucent form dances in the wind!",
                "It fades into vapor.",
                "Your scream is lost in the clouds."
            ),
        };
    
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> 
        crypt = new()
        {
            // Crypt
            (
                "Bone Knight",
                new() { "swings a cursed sword!", "raises a skeletal shield!" },
                "Chains clink—a knight of bone steps forth!",
                "Its armor clatters as it falls.",
                "Its blade finds your heart in the dark."
            ),
            (
                "Tomb Rat",
                new() { "gnaws rapidly!", "dives into your legs!" },
                "Scratching echoes—rats emerge in packs!",
                "It squeals and flees into shadows.",
                "You're consumed by a thousand bites."
            ),
            (
                "Wailing Spirit",
                new() { "screeches with agony!", "passes through your soul!" },
                "A cold presence chills your spine!",
                "It dissolves into a moan.",
                "Your sanity crumbles under its scream."
            ),
            (
                "Crypt Widow",
                new() { "crawls along the walls!", "sprays venomous mist!" },
                "You spot eight gleaming eyes in the dark!",
                "It collapses into brittle legs.",
                "You writhe as the venom takes hold."
            ),
            (
                "Ghoul Priest",
                new() { "chants a dark prayer!", "throws corrupted fire!" },
                "A twisted figure emerges from the altar!",
                "Its staff snaps as it falls.",
                "Your soul is sealed in darkness."
            ),
        };
    
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> 
        graveyard = new()
        {
            // Graveyard
            (
                "Zombie Peasant",
                new() { "moans and flails wildly!", "tries to bite you!" },
                "A half-buried corpse pulls itself free!",
                "It collapses into a heap of limbs.",
                "It drags you into the earth."
            ),
            (
                "Gravedigger",
                new() { "swings a rusted shovel!", "throws a clump of bones!" },
                "A pale man with hollow eyes steps forth!",
                "He retreats into the fog.",
                "He buries you next to the others."
            ),
            (
                "Carrion Crow",
                new() { "pecks furiously!", "calls a murder of crows!" },
                "Black wings blot out the moon!",
                "The crows scatter in defeat.",
                "You vanish beneath a flurry of beaks."
            ),
            (
                "Mourning Shade",
                new() { "cries softly and drains warmth!", "embraces you with frost!" },
                "You feel watched—grief takes shape!",
                "Its tears evaporate in silence.",
                "You die in cold embrace."
            ),
            (
                "Revenant",
                new() { "moves with relentless fury!", "howls your name!" },
                "You sense hatred—something remembers you!",
                "It dissolves in its vengeance spent.",
                "It finally claims what was owed."
            )

        };
    
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> 
        meadow = new()
        {
            // Meadow Field
            (
                "Wild Boar",
                new() { "charges with its tusks!", "snorts and kicks dirt!" },
                "Tall grass rustles—something approaches fast!",
                "It lets out a final squeal.",
                "You are gored and left behind."
            ),
            (
                "Honey Wasp",
                new() { "stings with precision!", "buzzes violently!" },
                "A buzzing swarm clouds the sky!",
                "It spirals into the distance.",
                "The venom numbs your heart."
            ),
            (
                "Field Sprite",
                new() { "sprinkles pollen dust!", "laughs and vanishes!" },
                "Glittering lights dance over flowers!",
                "It winks and fades like a dream.",
                "You wander, dazed, forever lost."
            ),
            (
                "Ram Spirit",
                new() { "clashes with ethereal horns!", "bleats a thunderous cry!" },
                "A ghostly ram stands in defiance!",
                "Its form crumbles like fog.",
                "It crushes your will and your bones."
            ),
            (
                "Sunflower Sentinel",
                new() { "whips with thorny vines!", "fires blinding seeds!" },
                "A towering plant turns to face you!",
                "Its petals wilt and drop.",
                "You're buried in golden petals."
            ),
        };

    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> 
        mansion = new()
        {
            // Royal Mansion
            (
                "Possessed Butler",
                new() { "hurls a silver tray!", "charges with a dinner knife!" },
                "A polite voice echoes: “Your final course...”",
                "He bows and vanishes in smoke.",
                "He straightens your body for display."
            ),
            (
                "Painted Phantom",
                new() { "steps out of a portrait!", "swipes with an oil-slick hand!" },
                "The painting's eyes move—then the frame shatters!",
                "Its strokes unravel like thread.",
                "Your face becomes its new canvas."
            ),
            (
                "Cursed Musician",
                new() { "strikes a dissonant chord!", "commands shadows with song!" },
                "A violin screeches from the ballroom!",
                "The strings snap, silencing all.",
                "You dance into death, step by step."
            ),
            (
                "Noble Shade",
                new() { "glares with regal fury!", "raises a spectral cane!" },
                "A ghost in royal garb blocks your way!",
                "He dissipates with a scoff.",
                "He claims your title and your life."
            ),
            (
                "Mansion Hound",
                new() { "leaps from a velvet cushion!", "bares glowing teeth!" },
                "A low growl echoes from the corridor!",
                "It whimpers and fades into dust.",
                "It mauls you beneath the chandeliers."
            ),
        };
    
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> 
        shore = new()
        {
            // Shore
            (
                "Crab Brute",
                new() { "snaps with iron claws!", "flips sand in your eyes!" },
                "A shadow scuttles from the tide!",
                "It retreats under a rock.",
                "You are cracked open like a shell."
            ),
            (
                "Sirensinger",
                new() { "sings a haunting melody!", "summons crashing waves!" },
                "A voice calls sweetly from the rocks!",
                "She disappears beneath the foam.",
                "You follow her song into the deep."
            ),
            (
                "Tide Serpent",
                new() { "lashes with watery coils!", "spits saltwater venom!" },
                "The sea hisses—a shape rises!",
                "It slips back into the surf.",
                "You drown before you ever scream."
            ),
            (
                "Drowned Sailor",
                new() { "swings a waterlogged anchor!", "groans your name!" },
                "Footsteps splash behind you—too late!",
                "He sinks back below the waves.",
                "He drags you to his eternal post."
            ),
            (
                "Jelly Bloom",
                new() { "glows blindingly!", "zaps with electric tentacles!" },
                "A soft light pulses beneath the water!",
                "Its form pops like a bubble.",
                "You twitch and vanish in the foam."
            )
        };
    
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> 
        snowyMountain = new()
        {
            // Snowy Mountain
            (
                "Frost Wolf",
                new() { "bites with icy fangs!", "howls to summon the pack!" },
                "A chilling howl cuts through the blizzard!",
                "It limps away into the snowdrift.",
                "You freeze beneath its glare."
            ),
            (
                "Ice Wraith",
                new() { "passes through you with frost!", "whispers freezing curses!" },
                "Mist forms a figure of dread!",
                "It shatters like glass.",
                "Your blood stills in your veins."
            ),
            (
                "Avalanche Beast",
                new() { "charges like a boulder!", "roars an echoing shockwave!" },
                "The snow trembles—something huge approaches!",
                "It tumbles off the cliff with a roar.",
                "You vanish under falling snow and fury."
            ),
            (
                "Frostbitten Hermit",
                new() { "swings a frozen staff!", "casts bitter wind!" },
                "A ragged figure steps from a cave!",
                "He collapses, frozen in place.",
                "You become part of his solitude."
            ),
            (
                "Glacial Harpy",
                new() { "dives with talons of ice!", "screeches a deafening cry!" },
                "Wings blot out the pale sun!",
                "It’s lost to the storm’s roar.",
                "You're dashed against jagged cliffs."
            ),
        };
    
    public static readonly List<(string name, List<string> attacks, string encounter, string death, string win)> 
        swamp = new()
        {
            // Swamp
            (
                "Bog Leech",
                new() { "lashes with a slimy tongue!", "latches on to drain life!" },
                "Ripples in the muck—something stirs!",
                "It bursts with a sickening pop.",
                "You vanish into the bog with it."
            ),
            (
                "Swamp Hag",
                new() { "cackles and throws curses!", "hurls vials of poison!" },
                "A crooked figure rises from the muck!",
                "She melts into the mire.",
                "Your lungs fill with her laughter."
            ),
            (
                "Mire Drake",
                new() { "spits acidic bile!", "snaps with jagged teeth!" },
                "Bubbles burst—a scaled back breaks the surface!",
                "It thrashes once and dies.",
                "You are dragged under its swampy coils."
            ),
            (
                "Rot Treant",
                new() { "flings chunks of rot!", "slams a festering root!" },
                "A tree groans unnaturally in the fog!",
                "It collapses into black mulch.",
                "Its spores choke your breath."
            ),
            (
                "Lurking Slime",
                new() { "splashes corrosive goo!", "absorbs your strikes silently!" },
                "Something shimmers beneath the waterline...",
                "It loses cohesion and melts away.",
                "You're dissolved where you stand."
            )
        };
}