#import "AweEstimoteiOS.h"
#import "AWEEstimoteBeacon.h"


@interface AweEstimoteiOS () <ESTBeaconManagerDelegate, ESTDeviceManagerDelegate, CBCentralManagerDelegate>

@property (nonatomic) ESTBeaconManager *beaconManager;
@property (nonatomic) CLBeaconRegion *beaconRegion;
@property (nonatomic) ESTDeviceManager *deviceManager;
@property (nonatomic) CBCentralManager *centralManager;

@end

@implementation AweEstimoteiOS

const char* gObject; // String to hold the gameobject needed for the UnitySendMessage.
bool promptBT;
bool btEnabled = true;
NSMutableArray<AWEEstimoteBeacon *> *beacons;
static AweEstimoteiOS* delegateObject = nil;


//Initialize logic for this plugin
- (id)init
{
    self = [super init];
    beacons = [[NSMutableArray alloc] init];
    return self;
}

- (void)applicationWillResignActive:(UIApplication *)application {
    // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
    // Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
}

- (void)applicationDidEnterBackground:(UIApplication *)application {
    // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
    // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
}

- (void)applicationWillEnterForeground:(UIApplication *)application {
    // Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
}

- (void)applicationDidBecomeActive:(UIApplication *)application {
    // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
}

- (void)applicationWillTerminate:(UIApplication *)application {
    // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
}


-(void)centralManagerDidUpdateState:(CBCentralManager *)central {
    if (central.state == CBCentralManagerStatePoweredOff){
        btEnabled = false;
    }
    else if (central.state == CBCentralManagerStatePoweredOn){
        btEnabled = true;
        //[delegateObject initializeBT];
    }
}

-(void)initializeBT{
    if(promptBT && btEnabled){
        _centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:dispatch_get_main_queue() options:@{CBCentralManagerOptionShowPowerAlertKey: @(NO)}];
        
        [self.beaconManager requestAlwaysAuthorization];
        CLAuthorizationStatus cla = [ESTBeaconManager authorizationStatus];
        if (cla == kCLAuthorizationStatusAuthorizedAlways){
            UnitySendMessage(gObject, "BTCallback", "true");
        }
        else if (cla == kCLAuthorizationStatusAuthorized){
            UnitySendMessage(gObject, "BTCallback", "true");
        }
        else{
            UnitySendMessage(gObject, "BTCallback", "BT Not Authorized");
        }
    }
    else if (!btEnabled){
        UnitySendMessage(gObject, "InitializationCallback", "Got Here");
        UnitySendMessage(gObject, "BTCallback", "BT disabled");
    }
    else if (btEnabled){
        UnitySendMessage(gObject, "BTCallback", "true");
    }
}

//Initialize plugin connection to Unity
-(void)initialize
{
    NSString* go = CreateNSString(gObject);
    NSString* appendedString = [NSString stringWithFormat:@"%s %@", "Starting init with",go];
    
    
    if (gObject != nil){
        UnitySendMessage(gObject, "InitializationCallback", [appendedString cStringUsingEncoding:NSASCIIStringEncoding]);
        self.beaconManager = [ESTBeaconManager new];
        self.beaconManager.delegate = self;
        
        self.deviceManager = [[ESTDeviceManager alloc] init];
        self.centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil];
        
        [delegateObject initializeBT];
        
        UnitySendMessage(gObject, "InitializationCallback", "Initialized");
    }
    else {
        UnitySendMessage(gObject, "InitializationCallback", "gObject null");
    }
}

//Initialize Estimote Cloud for this app
-(void)initializeCloudApp:(const char*)appId appToken:(const char*)appToken debugEstimoteCloud:(const bool*) debugEstimoteCloud uuid:(const char*) uuid
{
    NSString* ai = CreateNSString(appId);
    NSString* at = CreateNSString(appToken);
    NSString* uid = CreateNSString(uuid);
    BOOL bo = false;
    if (debugEstimoteCloud){
        bo = true;
    }
    
    
    if(![ai isEqualToString:@""] && ![at isEqualToString:@""] && ![uid isEqualToString:@""]){
        //Setup connection to Estimote Cloud
        [ESTConfig setupAppID: ai andAppToken: at];
        [ESTAnalyticsManager enableMonitoringAnalytics:bo];
        
        
        //Define which UUID to range for
        self.beaconRegion = [[CLBeaconRegion alloc] initWithProximityUUID:[[NSUUID alloc] initWithUUIDString: uid] identifier: @"rid"];
        
        UnitySendMessage(gObject, "CloudInitializationCallback", "Initialized");

    }
    else {
        NSString* appendString = [NSString stringWithFormat:@"%s %@ %@ %@", "Missing input from AWE_Estimote.cs. Have: ",ai, at, uid];
        UnitySendMessage(gObject, "CloudInitializationCallback", [appendString cStringUsingEncoding:NSASCIIStringEncoding]);   }
}

/*
-(void)beaconManager:(id)manager didChangeAuthorizationStatus:(CLAuthorizationStatus)status {
    
    if (status == kCLAuthorizationStatusAuthorizedAlways){
        UnitySendMessage(gObject, "CloudInitializationCallback", "Authorized Always");
    }
    else if (status == kCLAuthorizationStatusAuthorized){
        UnitySendMessage(gObject, "CloudInitializationCallback", "Authorized");
    }
    else {
        UnitySendMessage(gObject, "CloudInitializationCallback", "Not Authorized");
    }
}
*/
 
-(void)startScan
{
    //Begin ranging
    [self.beaconManager startRangingBeaconsInRegion:self.beaconRegion];
}

-(void)stopScan
{
    //Stop ranging
    [self.beaconManager stopRangingBeaconsInAllRegions];
}

-(void)beaconManager:(id)manager didRangeBeacons:(NSArray<CLBeacon *> *)beaconsInRange inRegion:(CLBeaconRegion *)region
{
    //UnitySendMessage(gObject, "CloudInitializationCallback", "didRangeBeacons");

    NSMutableString *beaconsInfo = [NSMutableString string];
    
    /*NSString* appendString = [NSString stringWithFormat:@"%s %@", "beacons count: ",[NSString stringWithFormat:@"%lu",(unsigned long)beacons.count]];
    UnitySendMessage(gObject, "CloudInitializationCallback", [appendString cStringUsingEncoding:NSASCIIStringEncoding]);*/
    
    for (CLBeacon* b in beaconsInRange){
        bool foundBeacon = false;
        
        for (AWEEstimoteBeacon* bea in beacons){
            if (bea.minor == b.minor && bea.major == b.major){
                foundBeacon = true;
                
                /*NSString* appendString = [NSString stringWithFormat:@"%s %@", "Found beacon with name: ",bea.beaconName];
                UnitySendMessage(gObject, "CloudInitializationCallback", [appendString cStringUsingEncoding:NSASCIIStringEncoding]);*/
                
                [beaconsInfo appendString:(@"|Name:")];
                [beaconsInfo appendString:(bea.beaconName)];
                
                [beaconsInfo appendString:(@"|MAC:")];
                [beaconsInfo appendString:(bea.macAddress)];
                
                [beaconsInfo appendString:(@"|UUID:")];
                [beaconsInfo appendString:([b.proximityUUID UUIDString])];
                
                [beaconsInfo appendString:(@"|Major:")];
                [beaconsInfo appendString:([b.major stringValue])];
                
                [beaconsInfo appendString:(@"|Minor:")];
                [beaconsInfo appendString:([b.minor stringValue])];
                
                [beaconsInfo appendString:(@"|RSSI:")];
                [beaconsInfo appendString:([@(b.rssi) stringValue])];
                
                [beaconsInfo appendString:(@"|MP:")];
                [beaconsInfo appendString:([NSString stringWithFormat:@"%ld",(long)bea.measuredPower])];
                
                [beaconsInfo appendString:(@"|Dist:")];
                [beaconsInfo appendString:([NSString stringWithFormat:@"%g",b.accuracy])];
                
                [beaconsInfo appendString:(@"|Color:")];
				[beaconsInfo appendString:(@"")];
				
				[beaconsInfo appendString:(@"|BatLife:")];
				[beaconsInfo appendString:(@"0")];
				
				[beaconsInfo appendString:(@"|Acc:")];
				[beaconsInfo appendString:(@"0")];
				[beaconsInfo appendString:(@",")];
				[beaconsInfo appendString:(@"0")];
				[beaconsInfo appendString:(@",")];
				[beaconsInfo appendString:(@"0")];
				
				[beaconsInfo appendString:(@"|Mag:")];
				[beaconsInfo appendString:(@"0")];
				[beaconsInfo appendString:(@",")];
				[beaconsInfo appendString:(@"0")];
				[beaconsInfo appendString:(@",")];
				[beaconsInfo appendString:(@"0")];
				
				[beaconsInfo appendString:(@"|Pressure:")];
				[beaconsInfo appendString:(@"0.00")];
				
				[beaconsInfo appendString:(@"|Light:")];
				[beaconsInfo appendString:(@"0.00")];
				
				[beaconsInfo appendString:(@"|Temp:")];
				[beaconsInfo appendString:(@"0.00")];
                
                [beaconsInfo appendString:(@";")];
                
                break;
            }
        }
        
        if (!foundBeacon){
            ESTRequestGetBeaconsDetails *beaconVO = [[ESTRequestGetBeaconsDetails alloc] initWithBeacons:beaconsInRange andFields:ESTBeaconDetailsAllFields];
            [beaconVO sendRequestWithCompletion:^(NSArray *beaconVOArray, NSError *error) {
                if (!error){
                    
                    
                    for (ESTBeaconVO *bvo in beaconVOArray){
                        if (bvo.major == b.major && bvo.minor == b.minor){
                            /*NSString* appendString = [NSString stringWithFormat:@"%s %@ %s %@",
                                                      "bvo minor:",
                                                      [NSString stringWithFormat:@"%@",bvo.minor],
                                                      "b minor:",
                                                      [NSString stringWithFormat:@"%@",b.minor]];
                            
                            UnitySendMessage(gObject, "InitializationCallback", [appendString cStringUsingEncoding:NSASCIIStringEncoding]);*/
                            
                            AWEEstimoteBeacon *eb = [[AWEEstimoteBeacon alloc ] init];
                            eb.beaconName = bvo.name;
                            eb.identifier = bvo.publicIdentifier;
                            eb.macAddress = bvo.macAddress;
                            eb.measuredPower = 0;
                            eb.proximityUUID = b.proximityUUID;
                            eb.major = b.major;
                            eb.minor = b.minor;
                            
                            [beacons addObject:eb];
                            
                            break;
                        }
                    }
                }
                else {
                    UnitySendMessage(gObject, "CloudInitializationCallback", [[NSString stringWithFormat:@"%@",error] cStringUsingEncoding:NSASCIIStringEncoding]);
                }
            }];
            
            [beaconsInfo appendString:(@"|Name:")];
            [beaconsInfo appendString:(@"")];
            
            [beaconsInfo appendString:(@"|MAC:")];
            [beaconsInfo appendString:(@"")];
            
            [beaconsInfo appendString:(@"|UUID:")];
            [beaconsInfo appendString:([b.proximityUUID UUIDString])];
            
            [beaconsInfo appendString:(@"|Major:")];
            [beaconsInfo appendString:([b.major stringValue])];
            
            [beaconsInfo appendString:(@"|Minor:")];
            [beaconsInfo appendString:([b.minor stringValue])];
            
            [beaconsInfo appendString:(@"|RSSI:")];
            [beaconsInfo appendString:([@(b.rssi) stringValue])];
            
            [beaconsInfo appendString:(@"|MP:")];
            [beaconsInfo appendString:(@"0")];
            
            [beaconsInfo appendString:(@"|Dist:")];
            [beaconsInfo appendString:([NSString stringWithFormat:@"%g",b.accuracy])];
            
            [beaconsInfo appendString:(@"|Color:")];
            [beaconsInfo appendString:(@"")];
            
            [beaconsInfo appendString:(@"|BatLife:")];
            [beaconsInfo appendString:(@"0")];
            
            [beaconsInfo appendString:(@"|Acc:")];
            [beaconsInfo appendString:(@"0")];
            [beaconsInfo appendString:(@",")];
            [beaconsInfo appendString:(@"0")];
            [beaconsInfo appendString:(@",")];
            [beaconsInfo appendString:(@"0")];
            
            [beaconsInfo appendString:(@"|Mag:")];
            [beaconsInfo appendString:(@"0")];
            [beaconsInfo appendString:(@",")];
            [beaconsInfo appendString:(@"0")];
            [beaconsInfo appendString:(@",")];
            [beaconsInfo appendString:(@"0")];
            
            [beaconsInfo appendString:(@"|Pressure:")];
            [beaconsInfo appendString:(@"0.00")];
            
            [beaconsInfo appendString:(@"|Light:")];
            [beaconsInfo appendString:(@"0.00")];
            
            [beaconsInfo appendString:(@"|Temp:")];
            [beaconsInfo appendString:(@"0.00")];
            
            [beaconsInfo appendString:(@";")];

        }
    }
    
    UnitySendMessage(gObject, "ScanListenerCallback", [beaconsInfo UTF8String]);
}

//Converts ESTColor to NSString
NSString* CreateStringFromColor (ESTColor color){
    NSString* tempColor;
    if (color == ESTColorMintCocktail){
        tempColor = @"Mint Cocktail";
    }
    else if (color == ESTColorIcyMarshmallow){
        tempColor = @"Icy Marshmallow";
    }
    else if (color == ESTColorBlueberryPie){
        tempColor = @"Blueberry Pie";
    }
    else if (color == ESTColorSweetBeetroot){
        tempColor = @"Sweet Beetroot";
    }
    else if (color == ESTColorCandyFloss){
        tempColor = @"Candy Floss";
    }
    else if (color == ESTColorLemonTart){
        tempColor = @"Lemon Tart";
    }
    else if (color == ESTColorVanillaJello){
        tempColor = @"Vanilla Jello";
    }
    else if (color == ESTColorLiquoriceSwirl){
        tempColor = @"Liquorice Swirl";
    }
    else if (color == ESTColorWhite){
        tempColor = @"White";
    }
    else if (color == ESTColorBlack){
        tempColor = @"Black";
    }
    else if (color == ESTColorTransparent){
        tempColor = @"Transparent";
    }
    
    return tempColor;
}

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}


@end




// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

//External code for Unity to work with
extern "C" {
    
    //This method is called from Unity with gameobject as value
    void _Initialize(const char* go, bool promptForBT)
    {
        gObject = MakeStringCopy(go);
        promptBT = promptForBT;
        
        //If this is the first time for initialization of an instance
        if(delegateObject == nil){
            delegateObject = [[AweEstimoteiOS alloc] init];
        }
        
        //Initialize this
        [delegateObject initialize];
    }
    
    //This method is called from Unity once Unity has received that this unity plugin has been initialized
    void _InitializeCloudApp(const char* appId, const char* appToken, const bool* debugEstimoteCloud, const char* uuid){
        if (delegateObject == nil){
            delegateObject = [[AweEstimoteiOS alloc] init];
        }
        
        [delegateObject initializeCloudApp:appId appToken:appToken debugEstimoteCloud:debugEstimoteCloud uuid:uuid];
    }
    
    void _StartScanning(){
        [delegateObject startScan];
    }
    
    void _StopScanning(){
        [delegateObject stopScan];
    }
}